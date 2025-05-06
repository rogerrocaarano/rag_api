using Application.UseCase;
using Domain.Repository;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Worker;

public class QueueSenderService(
    IMessagesRepository messagesRepository,
    IQueueMessagesRepository queueMessagesRepository,
    AskLlmUseCase askLlm,
    GetContextUseCase getContext
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var queuedMessages = await queueMessagesRepository.GetQueue();

                Console.WriteLine($"Sending {queuedMessages.Count} messages to LLM");
                foreach (var queuedMessage in queuedMessages)
                {
                    var message = await messagesRepository.GetMessage(queuedMessage.MessageId);
                    var conversationMessages =
                        await messagesRepository.GetMessagesByConversationId(message.ConversationId);
                    var context = await getContext.Execute(message.Content);
                    var llmResponse = await askLlm.Execute(conversationMessages, context);

                    await messagesRepository.AddMessage(
                        message.ConversationId,
                        llmResponse.Content,
                        Domain.Constant.MessageType.Assistant
                    );

                    await queueMessagesRepository.RemoveFromQueue(queuedMessage.Id);
                }

                await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en QueueSenderService: {ex.Message}");
            }
        }
    }
}
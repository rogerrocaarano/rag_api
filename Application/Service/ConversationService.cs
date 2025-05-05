using Application.UseCase;
using Domain.Constant;
using Domain.Model;
using Domain.Repository;

namespace Application.Service;

public class ConversationService(
    // IChatsRepository chats,
    IMessagesRepository messagesRepository,
    IConversationsRepository conversationsRepository,
    AskLlmUseCase askLlm,
    GetContextUseCase getContext)
{
    public async Task<Message> AgentQuery(string message, Guid userId)
    {
        var conversationId = conversationsRepository.AddConversation("Consulta del código de tránsito", userId).Result.Id;
        return await AgentQuery(message, conversationId, userId);
    }

    public async Task<Message> AgentQuery(string message, Guid conversationId, Guid userId)
    {
        await messagesRepository.AddMessage(conversationId, message, MessageType.User);
        var conversationHistory = conversationsRepository.GetConversation(conversationId).Result.Messages.ToList();
        var context = await getContext.Execute(message);
        var llmResponse = await askLlm.Execute(conversationHistory, context);
        var answer = await messagesRepository.AddMessage(conversationId, llmResponse.Content, MessageType.Assistant);
        return answer;
    }
}
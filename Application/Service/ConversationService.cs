using Application.UseCase;
using Domain.Constant;
using Domain.Model;
using Domain.Repository;

namespace Application.Service;

public class ConversationService(
    IChatsRepository chats,
    AskLlmUseCase askLlm,
    GetContextUseCase getContext)
{
    public async Task<Message> AgentQuery(string message, Guid userId)
    {
        var conversationId = chats.AddConversation("Consulta del código de tránsito", userId).Result.Id;
        return await AgentQuery(message, conversationId, userId);
    }

    public async Task<Message> AgentQuery(string message, Guid conversationId, Guid userId)
    {
        await chats.AddMessage(conversationId, message, MessageType.User);
        var conversationHistory = chats.GetConversation(conversationId).Result.Messages.ToList();
        var context = await getContext.Execute(message);
        var llmResponse = await askLlm.Execute(conversationHistory, context);
        var answer = await chats.AddMessage(conversationId, llmResponse.Content, MessageType.Assistant);
        return answer;
    }
}
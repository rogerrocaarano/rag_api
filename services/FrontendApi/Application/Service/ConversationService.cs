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
    public async Task<Message> AgentQuery(string message, Guid? userId = null)
    {
        var conversationId = chats.AddConversation("", userId).Result.Id;
        return await AgentQuery(message, conversationId, userId);
    }

    public async Task<Message> AgentQuery(string message, Guid conversationId, Guid? userId = null)
    {
        await chats.AddMessage(conversationId, message, MessageType.User);
        var conversationHistory = (List<Message>)chats.GetConversation(conversationId).Result.Messages;
        var context = await getContext.Execute(message);
        return await askLlm.Execute(conversationHistory, context);
    }
}
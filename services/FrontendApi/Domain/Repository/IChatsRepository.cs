using Domain.Model;

namespace Domain.Repository;

public interface IChatsRepository
{
    Task<Message> AddMessage(Guid conversationId, string messageContent, string messageRole);

    Task<Message> GetMessage(Guid messageId);

    Task DeleteMessage(Guid messageId);

    Task DeleteConversation(Guid conversationId);

    Task<Conversation> AddConversation(string name, Guid? ownerId);

    Task UpdateConversationName(Guid conversationId, string name);

    Task<Conversation> GetConversation(Guid conversationId);

    Task<List<Conversation>> GetConversationsByUserId(Guid userId);
}
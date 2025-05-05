using Domain.Model;

namespace Domain.Repository;

public interface IConversationsRepository
{
    Task<Conversation> AddConversation(string name, Guid? ownerId);

    Task UpdateConversationName(Guid conversationId, string name);

    Task<Conversation> GetConversation(Guid conversationId);

    Task<List<Conversation>> GetConversationsByUserId(Guid userId);

    Task DeleteConversation(Guid conversationId);
}
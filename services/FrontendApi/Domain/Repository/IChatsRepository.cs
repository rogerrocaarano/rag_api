using Domain.Model;

namespace Domain.Repository;

public interface IChatsRepository
{
    Task<Guid> AddMessage(Guid conversationId, string messageText, string messageRole);

    Task<Message> GetMessage(Guid messageId);

    Task DeleteMessage(Guid messageId);

    Task DeleteConversation(Guid conversationId);

    Task<Conversation> AddConversation(string name);

    Task UpdateConversation(Guid conversationId, Conversation conversation);

    Task<Conversation> GetConversation(Guid conversationId);

    Task<List<Message>> GetMessagesByConversationId(Guid conversationId);

    Task UpdateMessage(Message message);

    Task<Message> GetMessagesByUserId(Guid userId);
}
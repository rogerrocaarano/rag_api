using Domain.Model;

namespace Domain.Repository;

public interface IMessagesRepository
{
    Task<Message> AddMessage(Guid conversationId, string messageContent, string messageRole);

    Task<Message> GetMessage(Guid messageId);
    
    Task<List<Message>> GetMessagesByConversationId(Guid conversationId);

    Task DeleteMessage(Guid messageId);
}
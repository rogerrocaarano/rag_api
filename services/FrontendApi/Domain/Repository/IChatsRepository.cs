using Domain.Model;

namespace Domain.Repository;

public interface IChatsRepository
{
    BaseEntity AddMessage(Guid conversationId, String messageText, String messageRole);

    void GetMessage(Guid messageId, BaseEntity message);

    void DeleteMessage(Guid messageId);

    void DeleteConversation(Guid conversationId);

    BaseEntity AddConversation(String name);

    void UpdateConversation(Guid conversationId, String name);

    BaseEntity GetConversation(Guid conversationId);

    void GetMessagesByConversationId(Guid conversationId, List<BaseEntity> messages);

    void UpdateMessage();

    void GetMessagesByUserId();
}
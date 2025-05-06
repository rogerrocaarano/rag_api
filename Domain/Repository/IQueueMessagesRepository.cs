using Domain.Model;

namespace Domain.Repository;

public interface IQueueMessagesRepository
{
    Task<List<QueuedMessage>> GetQueue();
    Task<QueuedMessage> GetQueueById(Guid id);
    Task<QueuedMessage> AddToQueue(Guid messageId, Guid conversationId);
    Task<bool> RemoveFromQueue(Guid id);
}
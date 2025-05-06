using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository;

public class QueueMessagesRepository(ChatsDb dbContext) : IQueueMessagesRepository
{
    public async Task<List<QueuedMessage>> GetQueue()
    {
        var queue = await dbContext.QueuedMessages.ToListAsync();
        if (queue == null)
            throw new Exception("Queue not found");

        return queue;
    }

    public async Task<QueuedMessage> GetQueueById(Guid id)
    {
        var queuedMessage = await dbContext.QueuedMessages.FindAsync(id);
        if (queuedMessage == null)
            throw new Exception("Queued message not found");

        return queuedMessage;
    }

    public async Task<QueuedMessage> AddToQueue(Guid messageId, Guid conversationId)
    {
        var queuedMessage = new QueuedMessage
        {
            Id = Guid.NewGuid(),
            MessageId = messageId,
            ConversationId = conversationId
        };

        try
        {
            await dbContext.QueuedMessages.AddAsync(queuedMessage);
            await dbContext.SaveChangesAsync();
            return queuedMessage;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> RemoveFromQueue(Guid id)
    {
        var queuedMessage = await dbContext.QueuedMessages.FindAsync(id);
        if (queuedMessage == null)
            throw new Exception("Queued message not found");

        try
        {
            dbContext.QueuedMessages.Remove(queuedMessage);
            await dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
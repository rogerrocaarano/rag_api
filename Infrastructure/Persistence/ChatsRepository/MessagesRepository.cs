using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository;

public class MessagesRepository(ChatsDb dbContext) : IMessagesRepository
{
    public async Task<Message> AddMessage(Guid conversationId, string messageContent, string messageRole)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            Content = messageContent,
            Role = messageRole,
            CreatedAt = DateTime.UtcNow,
            ConversationId = conversationId
        };

        try
        {
            if (!await dbContext.Conversations.AnyAsync(c => c.Id == conversationId))
                throw new Exception("Conversation not found");

            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();
            return message;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Message> GetMessage(Guid messageId)
    {
        var message = await dbContext.Messages.FindAsync(messageId);
        if (message == null)
            throw new Exception("Message not found");

        return message;
    }

    public async Task DeleteMessage(Guid messageId)
    {
        var message = await dbContext.Messages.FindAsync(messageId);
        if (message == null)
            throw new Exception("Message not found");

        try
        {
            dbContext.Messages.Remove(message);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
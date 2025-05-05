using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository;

public class ConversationsRepository(ChatsDb dbContext) : IConversationsRepository
{
    public async Task<Conversation> AddConversation(string name, Guid? ownerId)
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Name = name,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow
        };

        try
        {
            await dbContext.Conversations.AddAsync(conversation);
            await dbContext.SaveChangesAsync();
            return conversation;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateConversationName(Guid conversationId, string name)
    {
        var conversation = await dbContext.Conversations.FindAsync(conversationId);
        if (conversation == null)
            throw new Exception("Conversation not found");

        try
        {
            conversation.Name = name;
            dbContext.Conversations.Update(conversation);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Conversation> GetConversation(Guid conversationId)
    {
        var conversation = await dbContext.Conversations
            .Include(c => c.Messages)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
            throw new Exception("Conversation not found");

        return conversation;
    }

    public async Task<List<Conversation>> GetConversationsByUserId(Guid userId)
    {
        var conversations = await dbContext.Conversations
            .Where(c => c.OwnerId == userId)
            .ToListAsync();

        if (!conversations.Any())
            throw new Exception("No conversations found for this user");

        return conversations;
    }

    public async Task DeleteConversation(Guid conversationId)
    {
        var conversation = await dbContext.Conversations.FindAsync(conversationId);
        if (conversation == null)
            throw new Exception("Conversation not found");

        try
        {
            dbContext.Conversations.Remove(conversation);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
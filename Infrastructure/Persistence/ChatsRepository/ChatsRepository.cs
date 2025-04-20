using Domain.Model;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository
{
    public class ChatsRepository(ChatsDb dbContext) : IChatsRepository
    {
        public async Task<Message> AddMessage(Guid conversationId, string messageContent, string messageRole)
        {
            // TODO: guardar la fecha recibida por la aplicación como SendAt
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
                if (!await ConversationExists(conversationId)) 
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
        
        private async Task<bool> ConversationExists(Guid conversationId)
        {
            var conversation = await dbContext.Conversations.FindAsync(conversationId);
            return conversation != null;
        }

        private async Task AddMessageToConversation(Guid conversationId, Message message)
        {
            var conversation = await dbContext.Conversations.FindAsync(conversationId);
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

            try
            {
                conversation.Messages.Add(message);
                dbContext.Conversations.Update(conversation);
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
            {
                throw new Exception("Message not found");
            }

            return message;
        }

        public async Task DeleteMessage(Guid messageId)
        {
            var message = await dbContext.Messages.FindAsync(messageId);
            if (message == null)
            {
                throw new Exception("Message not found");
            }

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

        public async Task DeleteConversation(Guid conversationId)
        {
            var conversation = await dbContext.Conversations.FindAsync(conversationId);
            if (conversation == null)
            {
                throw new Exception("Conversation not found");
            }

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
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return conversation;
        }

        public async Task UpdateConversationName(Guid conversationId, string name)
        {
            var conversation = new Conversation { Id = conversationId, Name = name };
            try
            {
                dbContext.Conversations.Attach(conversation);
                dbContext.Entry(conversation).Property(c => c.Name).IsModified = true;
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
            {
                throw new Exception("Conversation not found");
            }

            return conversation;
        }

        public async Task<List<Conversation>> GetConversationsByUserId(Guid userId)
        {
            var conversations = await dbContext.Conversations
                .Where(c => c.OwnerId == userId)
                .ToListAsync();

            if (conversations == null || !conversations.Any())
            {
                throw new Exception("No conversations found for this user");
            }

            return conversations;
        }
    }
}
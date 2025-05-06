using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ChatsRepository;

public class ChatsDb(DbContextOptions<ChatsDb> options) : DbContext(options)
{
    public DbSet<Message> Messages { get; set; }

    public DbSet<Conversation> Conversations { get; set; }
    
    public DbSet<FirebaseUser> FirebaseUsers { get; set; }
    
    public DbSet<QueuedMessage> QueuedMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>().HasKey(m => m.Id);
        modelBuilder.Entity<Conversation>().HasKey(c => c.Id);
        modelBuilder.Entity<Conversation>()
            .HasMany(c => c.Messages)
            .WithOne()
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<FirebaseUser>().HasKey(u => u.Id);
        modelBuilder.Entity<QueuedMessage>().HasKey(q => q.Id);
    }
}
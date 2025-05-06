namespace Domain.Model;

public class QueuedMessage : BaseEntity
{
    public Guid MessageId { get; set; }
    public Guid ConversationId { get; set; }
}
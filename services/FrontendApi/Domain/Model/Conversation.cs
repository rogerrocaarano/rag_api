namespace Domain.Model;

public class Conversation : BaseEntity
{
    public Guid? OwnerId { get; set; }

    public string Name { get; set; }

    public ICollection<Message> Messages { get; set; }
}
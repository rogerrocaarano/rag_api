namespace Domain.Model;

public class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }
}
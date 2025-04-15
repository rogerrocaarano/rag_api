namespace Domain.Model;

public class Fragment : BaseEntity
{
    public string Content { get; set; }

    public List<string> Tags { get; set; }

    public int SequenceId { get; set; }

    public Guid ContextId { get; set; }
}
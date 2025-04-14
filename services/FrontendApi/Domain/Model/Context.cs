namespace Domain.Model;

public class Context : Conversation
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<string> Tags { get; set; }
}
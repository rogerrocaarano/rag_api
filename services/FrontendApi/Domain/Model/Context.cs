namespace Domain.Model;

public class Context : BaseEntity
{
    public string Name { get; set; }

    public string Description { get; set; }

    public List<string> Tags { get; set; }
}
namespace Infrastructure.Persistence.ContextRepository.Model;

public class ChromaCollectionItem
{
    public Guid Id { get; set; }

    public List<float> Embedding { get; set; }

    public Dictionary<string, string> MetaData { get; set; }

    public string Document { get; set; }
}
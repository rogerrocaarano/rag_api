namespace Infrastructure.Provider.TextProcessor.Contract;

public class PostProcessEmbeddingResponse
{
    public required string Text { get; set; }
    public required List<float> Embedding { get; set; }
}
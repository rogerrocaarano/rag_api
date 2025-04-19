namespace Infrastructure.Provider.TextProcessor.Contract;

public class PostProcessTagsResponse
{
    public required string Text { get; set; }
    public required List<string> Tags { get; set; }
}
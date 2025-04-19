namespace Domain.Service;

public interface ITextProcessorService
{
    Task<List<float>> GenerateEmbeddingFromText(string text);

    Task<List<string>> SplitTextFile(string filePath);

    Task<List<string>> ExtractTagsFromText(string text);

    Task<List<string>> ExtractTagsFromText(string text, int maxTags);
}
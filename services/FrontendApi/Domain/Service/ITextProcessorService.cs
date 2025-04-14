using Domain.Model;

namespace Domain.Service;

public interface ITextProcessorService
{
    Task<EmbeddedText> GenerateEmbedding(string text);

    Task<List<string>> SplitText(string text);
}
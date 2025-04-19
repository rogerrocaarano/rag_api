namespace Domain.Service;

public interface ITextProcessorService {

    /// <summary>
    /// @param text 
    /// @return
    /// </summary>
    Task<List<float>> GenerateEmbeddingFromText(string text);

    /// <summary>
    /// @param text 
    /// @return
    /// </summary>
    Task<List<string>> SplitText(string text);

    /// <summary>
    /// @param text 
    /// @return
    /// </summary>
    Task<List<string>> ExtractTagsFromText(string text);

}
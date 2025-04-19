using Domain.Model;

namespace Domain.Repository;

public interface IContextRepository
{
    Task<Guid> AddContext(string name, string description, List<string> tags, List<float> embedding);

    Task<Guid> AddFragment(string content, List<string> tags, int sequenceId, Guid contextId, List<float> embedding);

    Task<Context> GetContext(Guid contextId);
    
    Task<Guid?> GetContextIdByFilePath(string name);

    Task<Fragment> GetFragment(Guid fragmentId);

    Task UpdateContextTags(Guid contextId, List<string> tags);

    Task DeleteContext(Guid contextId);

    Task DeleteFragment(Guid fragmentId);

    Task UpdateFragmentTags(Guid fragmentId, List<string> tags);

    Task<List<Fragment>> GetSimilarFragmentsByEmbedding(List<float> embedding);

    Task<List<Context>> GetSimilarContextsByEmbedding(List<float> embedding);
}
using ChromaDB.Client;
using Domain.Model;
using Domain.Repository;
using Infrastructure.Persistence.ContextRepository.Model;

namespace Infrastructure.Persistence.ContextRepository;

public class ContextRepository : IContextRepository
{
    private ChromaClient vectorDb { get; set; }

    private ChromaCollection contextCollection { get; set; }

    private ChromaCollection FragmentCollection { get; set; }

    public async Task<Guid> AddContext(string name, string description, List<string> tags)
    {
        throw new NotImplementedException();
    }

    public async Task<Guid> AddFragment(string content, List<string> tags, int sequenceId, Guid contextId)
    {
        throw new NotImplementedException();
    }

    public async Task<Context> GetContext(Guid contextId)
    {
        throw new NotImplementedException();
    }

    public async Task<Fragment> GetFragment(Guid fragmentId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateContextTags(Guid contextId, List<string> tags)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteContext(Guid contextId)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteFragment(Guid fragmentId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateFragmentTags(Guid fragmentId, List<string> tags)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Fragment>> GetSimilarFragmentsByEmbedding(List<float> embedding)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Context>> GetSimilarContextsByEmbedding(List<float> embedding)
    {
        throw new NotImplementedException();
    }
}
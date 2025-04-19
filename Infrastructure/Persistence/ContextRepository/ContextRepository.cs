using ChromaDB.Client;
using Domain.Model;
using Domain.Repository;
using Infrastructure.Persistence.ContextRepository.Builder;

namespace Infrastructure.Persistence.ContextRepository;

public class ContextRepository : IContextRepository
{
    private readonly ChromaCollectionClient _context;

    private readonly ChromaCollectionClient _fragments;

    public ContextRepository(
        string chromaUri,
        string contextCollectionName,
        string fragmentCollectionName
    )
    {
        var configOptions = new ChromaConfigurationOptions(uri: chromaUri);
        using var httpClient = new HttpClient();
        var vectorDb = new ChromaClient(configOptions, httpClient);

        var contextCollection = vectorDb.GetOrCreateCollection(contextCollectionName).Result;
        var fragmentCollection = vectorDb.GetOrCreateCollection(fragmentCollectionName).Result;

        _context = new ChromaCollectionClient(contextCollection, configOptions, httpClient);
        _fragments = new ChromaCollectionClient(fragmentCollection, configOptions, httpClient);
    }

    public async Task<Guid> AddContext(string name, string description, List<string> tags, List<float> embedding)
    {
        try
        {
            var id = Guid.NewGuid();
            await _context.Add(
                ids: [id.ToString()],
                documents: [description],
                metadatas:
                [
                    new Dictionary<string, object>
                    {
                        { "Name", name },
                        { "Description", description },
                        { "Tags", string.Join(",", tags) },
                        { "FirstFragment", Guid.Empty.ToString() },
                        { "FragmentCount", 0 }
                    }
                ],
                embeddings: [ItemBuilder.ConvertToReadOnlyMemory(embedding)]
            );
            return id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Guid> AddFragment(string content, List<string> tags, int sequenceId, Guid contextId,
        List<float> embedding)
    {
        try
        {
            var id = Guid.NewGuid();
            await _fragments.Add(
                ids: [id.ToString()],
                documents: [content],
                metadatas:
                [
                    new Dictionary<string, object>
                    {
                        { "Tags", string.Join(",", tags) },
                        { "SequenceId", sequenceId },
                        { "ContextId", contextId.ToString() }
                    }
                ],
                embeddings: [ItemBuilder.ConvertToReadOnlyMemory(embedding)]
            );
            return id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Context> GetContext(Guid contextId)
    {
        try
        {
            var result = await _context.Get(contextId.ToString());
            return result == null
                ? throw new Exception($"Context with ID {contextId} not found.")
                : ItemBuilder.ItemToContext(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Fragment> GetFragment(Guid fragmentId)
    {
        try
        {
            var result = await _fragments.Get(fragmentId.ToString());
            return result == null
                ? throw new Exception($"Fragment with ID {fragmentId} not found.")
                : ItemBuilder.ItemToFragment(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateContextTags(Guid contextId, List<string> tags)
    {
        try
        {
            var entry = await _context.Get(contextId.ToString());
            if (entry == null)
                throw new Exception($"Context with ID {contextId} not found.");
            var metadata = entry.Metadata;
            metadata["Tags"] = string.Join(",", tags);
            await _context.Update(ids: [contextId.ToString()], metadatas: [metadata]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteContext(Guid contextId)
    {
        try
        {
            var context = await _context.Get(contextId.ToString());
            if (context == null)
                throw new Exception($"Context with ID {contextId} not found.");

            // get all fragments for the context
            var fragments = await _fragments.Get(where: ChromaWhereOperator.Equal("ContextId", contextId.ToString()));
            var fragmentIds = fragments.Select(f => f.Id).ToList();
            // delete all fragments
            await _fragments.Delete(fragmentIds);
            // delete the context
            await _context.Delete([contextId.ToString()]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteFragment(Guid fragmentId)
    {
        try
        {
            var entry = await _fragments.Get(fragmentId.ToString());
            if (entry == null)
                throw new Exception($"Fragment with ID {fragmentId} not found.");
            await _fragments.Delete([fragmentId.ToString()]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task UpdateFragmentTags(Guid fragmentId, List<string> tags)
    {
        try
        {
            var entry = await _fragments.Get(fragmentId.ToString());
            if (entry == null)
                throw new Exception($"Fragment with ID {fragmentId} not found.");
            var metadata = entry.Metadata;
            metadata["Tags"] = string.Join(",", tags);
            await _fragments.Update(ids: [fragmentId.ToString()], metadatas: [metadata]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Fragment>> GetSimilarFragmentsByEmbedding(List<float> embedding)
    {
        try
        {
            var queryResult = await _fragments.Query(queryEmbeddings: ItemBuilder.ConvertToReadOnlyMemory(embedding));
            var fragmentIds = queryResult
                .Where(x => x.Distance > 0.5)
                .OrderBy(x => x.Distance)
                .Select(x => Guid.Parse(x.Id))
                .ToList();

            var fragments = new List<Fragment>();
            foreach (var fragmentId in fragmentIds)
            {
                var fragment = await GetFragment(fragmentId);
                fragments.Add(fragment);
            }

            return fragments;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<List<Context>> GetSimilarContextsByEmbedding(List<float> embedding)
    {
        try
        {
            var queryResult = await _context.Query(queryEmbeddings: ItemBuilder.ConvertToReadOnlyMemory(embedding));
            var contextIds = queryResult
                .Where(x => x.Distance > 0.5)
                .OrderBy(x => x.Distance)
                .Select(x => Guid.Parse(x.Id))
                .ToList();

            var contexts = new List<Context>();
            foreach (var contextId in contextIds)
            {
                var context = await GetContext(contextId);
                contexts.Add(context);
            }

            return contexts;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
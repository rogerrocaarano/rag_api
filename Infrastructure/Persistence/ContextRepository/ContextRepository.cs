using ChromaDB.Client;
using Domain.Model;
using Domain.Repository;
using Infrastructure.Persistence.ContextRepository.Builder;

namespace Infrastructure.Persistence.ContextRepository;

public class ContextRepository : IContextRepository
{
    private ChromaClient _chromaClient;
    private readonly HttpClient _httpClient;
    private ChromaCollectionClient? _contextCollection;
    private ChromaCollectionClient? _fragmentCollection;
    private ChromaConfigurationOptions _chromaConfiguration;

    public ContextRepository(
        string chromaUri,
        HttpClient httpClient
    )
    {
        _chromaConfiguration =
            new ChromaConfigurationOptions(
                uri: chromaUri,
                defaultTenant: "vialmentor",
                defaultDatabase: "app"
            );
        _chromaClient = new ChromaClient(_chromaConfiguration, httpClient);
        _httpClient = httpClient;
    }

    public async Task InitializeAsync(string contextCollectionName, string fragmentCollectionName)
    {
        try
        {
            var contextCollection = await _chromaClient.GetOrCreateCollection(contextCollectionName);
            _contextCollection = new ChromaCollectionClient(
                contextCollection,
                _chromaConfiguration,
                _httpClient
            );

            var fragmentCollection = await _chromaClient.GetOrCreateCollection(fragmentCollectionName);
            _fragmentCollection = new ChromaCollectionClient(
                fragmentCollection,
                _chromaConfiguration,
                _httpClient
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Guid> AddContext(string name, string description, List<string> tags, List<float> embedding)
    {
        try
        {
            var id = Guid.NewGuid();
            await _contextCollection.Add(
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
            await _fragmentCollection.Add(
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
            var entry = await _contextCollection.Get(contextId.ToString());
            return entry == null
                ? throw new Exception($"Context with ID {contextId} not found.")
                : ItemBuilder.ItemToContext(entry);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Guid?> GetContextIdByFilePath(string name)
    {
        try
        {
            var result = await _contextCollection.Get(name);
            return result == null
                ? null
                : Guid.Parse(result.Id);
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
            var result = await _fragmentCollection.Get(fragmentId.ToString());
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
            var entry = await _contextCollection.Get(contextId.ToString());
            if (entry == null)
                throw new Exception($"Context with ID {contextId} not found.");
            var metadata = entry.Metadata;
            metadata["Tags"] = string.Join(",", tags);
            await _contextCollection.Update(ids: [contextId.ToString()], metadatas: [metadata]);
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
            var context = await _contextCollection.Get(contextId.ToString());
            if (context == null)
                throw new Exception($"Context with ID {contextId} not found.");

            // get all fragments for the context
            var fragments =
                await _fragmentCollection.Get(where: ChromaWhereOperator.Equal("ContextId", contextId.ToString()));
            var fragmentIds = fragments.Select(f => f.Id).ToList();
            // delete all fragments
            await _fragmentCollection.Delete(fragmentIds);
            // delete the context
            await _contextCollection.Delete([contextId.ToString()]);
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
            var entry = await _fragmentCollection.Get(fragmentId.ToString());
            if (entry == null)
                throw new Exception($"Fragment with ID {fragmentId} not found.");
            await _fragmentCollection.Delete([fragmentId.ToString()]);
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
            var entry = await _fragmentCollection.Get(fragmentId.ToString());
            if (entry == null)
                throw new Exception($"Fragment with ID {fragmentId} not found.");
            var metadata = entry.Metadata;
            metadata["Tags"] = string.Join(",", tags);
            await _fragmentCollection.Update(ids: [fragmentId.ToString()], metadatas: [metadata]);
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
            var queryResult =
                await _fragmentCollection.Query(queryEmbeddings: ItemBuilder.ConvertToReadOnlyMemory(embedding));
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
            var entries =
                await _contextCollection.Query(queryEmbeddings: ItemBuilder.ConvertToReadOnlyMemory(embedding));
            var contextIds = entries
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
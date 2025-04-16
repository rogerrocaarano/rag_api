using ChromaDB.Client.Models;
using Domain.Model;

namespace Infrastructure.Persistence.ContextRepository.Builder;

public static class ItemBuilder
{
    public static ReadOnlyMemory<float> ConvertToReadOnlyMemory(List<float> embedding)
    {
        var floatArray = embedding.ToArray();
        return new ReadOnlyMemory<float>(floatArray);
    }
    
    public static ChromaCollectionEntry ContextToItem(Context context, List<float> embedding)
    {
        return new ChromaCollectionEntry(context.Id.ToString())
        {
            Document = context.Description,
            Metadata = new Dictionary<string, object>
            {
                { "Name", context.Name },
                { "Tags", string.Join(",", context.Tags) },
                { "Embedding", embedding },
                { "FirstFragment", context.Fragments.First().Id.ToString() ?? "" },
                { "FragmentCount", context.Fragments.Count },
            },
            Embeddings = ConvertToReadOnlyMemory(embedding)
        };
    }


    public static ChromaCollectionEntry FragmentToItem(Fragment fragment, List<float> embedding)
    {
        return new ChromaCollectionEntry(fragment.Id.ToString())
        {
            Document = fragment.Content,
            Metadata = new Dictionary<string, object>
            {
                { "Tags", string.Join(",", fragment.Tags) },
                { "SequenceId", fragment.SequenceId },
                { "ContextId", fragment.ContextId.ToString() },
            },
            Embeddings = ConvertToReadOnlyMemory(embedding)
        };
    }

    public static Context ItemToContext(ChromaCollectionEntry item)
    {
        return new Context
        {
            Id = Guid.Parse(item.Id),
            Name = item.Document ?? "",
            Description = item.Metadata["Description"].ToString() ?? "",
            Tags = item.Metadata["Tags"].ToString().Split(',').ToList(),
            Fragments = new List<Fragment>()
        };
    }

    public static Fragment ItemToFragment(ChromaCollectionEntry item)
    {
        return new Fragment
        {
            Id = Guid.Parse(item.Id),
            Content = item.Document ?? "",
            Tags = item.Metadata["Tags"].ToString().Split(',').ToList(),
            SequenceId = Convert.ToInt32(item.Metadata["SequenceId"]),
            ContextId = Guid.Parse(item.Metadata["ContextId"].ToString()),
        };
    }
}
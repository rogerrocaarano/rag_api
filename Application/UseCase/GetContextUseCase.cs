using Application.Dto;
using Domain.Repository;
using Domain.Service;

namespace Application.UseCase;

public class GetContextUseCase(
    IContextRepository contextDb,
    ITextProcessorService textProcessor
)

{
    public async Task<GetContextDto> Execute(string query)
    {
        var queryTags = await textProcessor.ExtractTagsFromText(query);
        var embedding = await textProcessor.GenerateEmbeddingFromText(string.Join(',', queryTags));
        var similarFragments = await contextDb.GetSimilarFragmentsByEmbedding(embedding);
        return new GetContextDto()
        {
            Fragments = similarFragments,
            QueryTags = queryTags
        };
    }
}
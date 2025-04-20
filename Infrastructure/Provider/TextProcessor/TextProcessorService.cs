using Domain.Service;
using Infrastructure.Provider.TextProcessor.Constant;
using Infrastructure.Provider.TextProcessor.Contract;
using RestSharp;

namespace Infrastructure.Provider.TextProcessor;

public class TextProcessorService : ITextProcessorService
{
    private readonly RestClient _client;

    public TextProcessorService(string baseUrl)
    {
        var options = new RestClientOptions(baseUrl);
        _client = new RestClient(options);
    }

    public async Task<List<float>> GenerateEmbeddingFromText(string text)
    {
        var restRequest = new RestRequest(ApiEndpoint.Embedding);
        restRequest.AddParameter("text", text);
        var restResponse = await _client.GetAsync<PostProcessEmbeddingResponse>(restRequest);

        return restResponse.Embedding;
    }

    public async Task<List<string>> SplitTextFile(string filePath)
    {
        try
        {
            var restRequest = new RestRequest(ApiEndpoint.SplitFile, Method.Post);
            restRequest.AddFile("file", filePath, "text/plain");
            
            var response = await _client.PostAsync<List<string>>(restRequest);
            return response ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public async Task<List<string>> ExtractTagsFromText(string text)
    {
        var restRequest = new RestRequest(ApiEndpoint.Tags);
        restRequest.AddParameter("text", text);
        var restResponse = await _client.GetAsync<PostProcessTagsResponse>(restRequest);

        return restResponse.Tags;
    }

    public async Task<List<string>> ExtractTagsFromText(string text, int maxTags)
    {
        var restRequest = new RestRequest(ApiEndpoint.MostCommonTags);
        restRequest.AddParameter("text", text);
        restRequest.AddParameter("n", maxTags);
        var restResponse = await _client.GetAsync<PostProcessTagsResponse>(restRequest);

        return restResponse.Tags;
    }
}
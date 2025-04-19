using Domain.Model;
using Domain.Service;
using Infrastructure.Provider.Deepseek.Builder;
using Infrastructure.Provider.Deepseek.Constant;
using Infrastructure.Provider.Deepseek.Contract;
using RestSharp;

namespace Infrastructure.Provider.Deepseek;

public class DeepseekClient : IDisposable, ILlmService
{
    private readonly RestClient _client;

    private readonly string _apiKey;

    public DeepseekClient(string apiKey)
    {
        var options = new RestClientOptions(ApiEndpoint.BaseUrl);
        _client = new RestClient(options);
        _apiKey = apiKey;
    }

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<ChatCompletionResponse?> PostCompletionChat(ChatCompletionRequest request)
    {
        try
        {
            var restRequest = new RestRequest(ApiEndpoint.ChatCompletions, Method.Post);
            var headers = new List<KeyValuePair<string, string>>
            {
                KeyValuePair.Create("Content-Type", ContentType.Json.Value),
                KeyValuePair.Create("Authorization", $"Bearer {_apiKey}")
            };
            restRequest.AddHeaders(headers);
            restRequest.AddJsonBody(request);

            var restResponse = await _client.PostAsync<ChatCompletionResponse>(restRequest);
            return restResponse;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<Message> AnswerQuestion(List<Message> conversation, List<Fragment> context, Message rules)
    {
        var request = ChatCompletionBuilder.BuildRequest(conversation, context, rules);
        var response = await PostCompletionChat(request);
        return new Message
        {
            // TODO: revisar si el response es null
            Content = response.Choices[0].Message.Content,
            Role = Domain.Constant.MessageType.Assistant,
            CreatedAt = DateTime.Now
        };
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}
using Infrastructure.Provider.Deepseek.Model;

namespace Infrastructure.Provider.Deepseek.Contract;

public class ChatCompletionResponse
{
    public string Id { get; set; }

    public string Model { get; set; }

    public List<DsChoice> Choices { get; set; }
}
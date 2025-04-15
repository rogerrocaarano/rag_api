using Infrastructure.Provider.Deepseek.Model;

namespace Infrastructure.Provider.Deepseek.Contract;

public class ChatCompletionRequest
{
    public List<DsMessage> Messages { get; set; }

    public bool Stream { get; set; }

    public String Model { get; set; }
}
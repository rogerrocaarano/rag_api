namespace Presentation.RestApi.Contract;

public class SendChatMessageRequest
{
    public Guid? ConversationId { get; set; }

    public required string Message { get; set; }
}
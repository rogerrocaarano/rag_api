namespace Presentation.RestApi.Contract.Conversations;

public record GetMessageResponse(
    Guid id,
    Guid conversationId,
    string text,
    string role,
    DateTime createdAt
);
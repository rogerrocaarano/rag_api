using Application.Service;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Presentation.RestApi.Contract;

namespace Presentation.RestApi.Controller;

[Route("[controller]")]
[Controller]
public class ChatAgentController(ConversationService conversationService) : ControllerBase
{
    // TODO: Implementar un sistema de autenticación y autorización
    private static readonly Guid DefaultUserId = new("00000000-0000-0000-0000-000000000001");

    [HttpPost("send-message")]
    public async Task<IActionResult> PostSendMessage(SendChatMessageRequest request)
    {
        try
        {
            var answer = request.ConversationId == null
                ? await conversationService.AgentQuery(message: request.Message, userId: DefaultUserId)
                : await conversationService.AgentQuery(request.Message, (Guid)request.ConversationId, DefaultUserId);

            var response = new SendChatMessageResponse
            {
                message = answer.Content,
                conversationId = answer.ConversationId,
                createdAt = answer.CreatedAt
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}
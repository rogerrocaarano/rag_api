using Application.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.RestApi.Contract.Conversations;

namespace Presentation.RestApi.Controller;

[Route("[controller]")]
[Controller]
public class ConversationsController(ConversationService conversationService) : ControllerBase
{
    [HttpPost("create")]
    [Authorize(AuthenticationSchemes = "Firebase")]
    public async Task<IActionResult> PostCreateConversation([FromBody] CreateRequest request)
    {
        var firebaseId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(firebaseId))
        {
            return Unauthorized("Invalid Firebase user.");
        }

        try
        {
            var conversationId = await conversationService.StartConversation(request.conversationHeader, firebaseId);

            return Ok(new { conversationId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }

    [HttpPost("{conversationId}/add-message")]
    [Authorize(AuthenticationSchemes = "Firebase")]
    public async Task<IActionResult> PostAddMessage([FromBody] AddMessageRequest request, [FromRoute]string conversationId)
    {
        var firebaseId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(firebaseId))
        {
            return Unauthorized("Invalid Firebase user.");
        }

        try
        {
            var messageId = await conversationService.AddMessageToConversation(
                request.content,
                Guid.Parse(conversationId),
                firebaseId
            );
            return Ok(new { messageId });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}
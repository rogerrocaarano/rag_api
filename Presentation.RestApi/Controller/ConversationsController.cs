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
    public async Task<IActionResult> CreateConversation([FromBody] CreateRequest request)
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
    public async Task<IActionResult> AddMessage([FromBody] AddMessageRequest request, [FromRoute] string conversationId)
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

    [HttpGet("{conversationId}/messages")]
    [Authorize(AuthenticationSchemes = "Firebase")]
    public async Task<IActionResult> GetMessages([FromRoute] string conversationId)
    {
        var firebaseId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(firebaseId))
        {
            return Unauthorized("Invalid Firebase user.");
        }

        try
        {
            var messagesIds =
                await conversationService.GetConversationMessagesIds(firebaseId, Guid.Parse(conversationId));
            return Ok(messagesIds);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }

    [HttpGet("{conversationId}/messages/{messageId}")]
    [Authorize(AuthenticationSchemes = "Firebase")]
    public async Task<IActionResult> GetMessage([FromRoute] string conversationId, [FromRoute] string messageId)
    {
        var firebaseId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(firebaseId))
        {
            return Unauthorized("Invalid Firebase user.");
        }

        try
        {
            var message = await conversationService.GetMessageById(firebaseId, Guid.Parse(messageId));
            if (message == null) 
                return NotFound();
            
            var response = new GetMessageResponse(
                id: message.Id,
                conversationId: message.ConversationId,
                text: message.Content,
                role: message.Role,
                createdAt: message.CreatedAt
            );
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Problem(e.Message);
        }
    }
}
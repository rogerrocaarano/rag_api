using Domain.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.RestApi.Controller;

[ApiController]
[Route("[controller]")]
public class UserRegistrationController(IFirebaseUsersRepository firebaseUsers) : ControllerBase
{
    [HttpGet("register-firebase-user")]
    [Authorize(AuthenticationSchemes = "Firebase")]
    public async Task<IActionResult> RegisterFirebaseUserIfNotExists()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("Invalid Firebase user.");
        }

        // add user to database if not exists
        var userGuid = await firebaseUsers.GetUserByFirebaseId(userId) ?? 
                       await firebaseUsers.AddFirebaseUser(userId);

        return Ok($"User registered successfully with ID: {userGuid}");
    }
}
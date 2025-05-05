using FirebaseAdmin.Auth;

namespace Presentation.RestApi.Middleware.Firebase;

public class FirebaseAuthMiddleware
{
    private readonly RequestDelegate _next;
    public FirebaseAuthMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext ctx)
    {
        var authHeader = ctx.Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader?.StartsWith("Bearer ") == true)
        {
            var idToken = authHeader.Substring("Bearer ".Length);
            try
            {
                var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
                ctx.Items["uid"] = decoded.Uid;
            }
            catch
            {
                ctx.Response.StatusCode = 401;
                await ctx.Response.WriteAsync("Token inválido");
                return;
            }
        }

        await _next(ctx);
    }
}
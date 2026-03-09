namespace BunkerMVP.API.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        // Allow swagger and auth endpoints
        if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase) ||
            path.StartsWith("/api/auth", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        // Protect all /api/* routes
        if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase))
        {
            var userId = context.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\":\"Unauthorized\"}");
                return;
            }
        }

        await _next(context);
    }
}

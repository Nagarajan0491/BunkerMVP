namespace BunkerMVP.API.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string? _apiKey;

    public AuthMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration["ChatbotSystem:ApiKey"];
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
            // API key bypass for machine-to-machine calls (chatbot system)
            if (!string.IsNullOrWhiteSpace(_apiKey) &&
                context.Request.Headers.TryGetValue("X-Api-Key", out var providedKey) &&
                providedKey.ToString() == _apiKey)
            {
                await _next(context);
                return;
            }

            // Session auth for browser requests
            await context.Session.LoadAsync();
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

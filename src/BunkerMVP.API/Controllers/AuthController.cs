using BunkerMVP.Application.DTOs.Auth;
using BunkerMVP.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace BunkerMVP.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
            return Unauthorized(result);

        HttpContext.Session.SetString("UserId", result.User!.Id.ToString());
        HttpContext.Session.SetString("Username", result.User.Username);
        return Ok(result);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = HttpContext.Session.GetString("UserId");
        var username = HttpContext.Session.GetString("Username");
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "Not authenticated" });

        return Ok(new { id = userId, username });
    }
}

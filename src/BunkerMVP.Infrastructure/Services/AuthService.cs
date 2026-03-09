using BunkerMVP.Application.DTOs.Auth;
using BunkerMVP.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BunkerMVP.Infrastructure.Services;

public class AuthService
{
    private readonly BunkerDbContext _db;

    public AuthService(BunkerDbContext db)
    {
        _db = db;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _db.AdminUsers
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
        {
            return new LoginResponse { Success = false, Message = "Invalid username or password" };
        }

        return new LoginResponse
        {
            Success = true,
            Message = "Login successful",
            User = new UserInfo { Id = user.Id, Username = user.Username, FullName = user.FullName }
        };
    }
}

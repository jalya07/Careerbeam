using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Careerbeam.Core.DTOs;
using Careerbeam.Core.Entities;
using Careerbeam.Core.Interfaces;
using Careerbeam.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Careerbeam.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly CareerbeamDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(CareerbeamDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        // Проверяем, нет ли уже такого email
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = request.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync(); // нужно сохранить, чтобы получить user.Id

        // Сразу создаём соответствующий профиль
        if (request.Role == UserRole.JobSeeker)
        {
            _context.JobSeekerProfiles.Add(new JobSeekerProfile
            {
                UserId = user.Id,
                FullName = request.FullName
            });
        }
        else if (request.Role == UserRole.Employer)
        {
            _context.EmployerProfiles.Add(new EmployerProfile
            {
                UserId = user.Id,
                CompanyName = request.FullName
            });
        }

        await _context.SaveChangesAsync();

        var token = GenerateToken(user);
        return new AuthResponse { Token = token, Email = user.Email, Role = user.Role };
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный email или пароль.");

        var token = GenerateToken(user);
        return new AuthResponse { Token = token, Email = user.Email, Role = user.Role };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_config["Jwt:ExpiresInMinutes"]!)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
using Careerbeam.Core.DTOs;

namespace Careerbeam.Core.Interfaces;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<CurrentUserResponse> GetCurrentUserAsync(int userId);
}
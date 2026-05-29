using CareerPilot.API.DTOs;

namespace CareerPilot.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> Register(RegisterDto dto);
        Task<AuthResponseDto?> Login(LoginDto dto);
    }
}
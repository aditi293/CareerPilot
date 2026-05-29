using CareerPilot.API.DTOs;
using CareerPilot.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CareerPilot.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterDto dto)
        {
            var result = await _authService.Register(dto);

            if (result == null)
                return BadRequest("Email already exists");

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.Login(dto);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }
    }
}
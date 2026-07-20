using Microsoft.AspNetCore.Mvc;
using SmartKitchenInventoryAPI.DTOs;
using SmartKitchenInventoryAPI.Interfaces;

namespace SmartKitchenInventoryAPI.Controllers
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

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result == "Username already exists.")
            {
                return BadRequest(new
                {
                    Message = result
                });
            }

            return Ok(new
            {
                Message = result
            });
        }

        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var token = await _authService.LoginAsync(request);

            if (token == null)
            {
                return Unauthorized(new
                {
                    Message = "Invalid username or password."
                });
            }

            return Ok(new
            {
                Token = token
            });
        }
    }
}
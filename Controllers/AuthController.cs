using Microsoft.AspNetCore.Mvc;
using SmartKitchenInventoryAPI.DTOs;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto request)
        {
            
            var users = new List<User>
            {
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Password = "Admin@123",
                    Role = "Admin"
                },
                new User
                {
                    Id = 2,
                    Username = "employee",
                    Password = "Employee@123",
                    Role = "Employee"
                }
            };

            var user = users.FirstOrDefault(u =>
                u.Username == request.Username &&
                u.Password == request.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _jwtService.GenerateToken(user);

            var response = new LoginResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            };

            return Ok(response);
        }
    }
}
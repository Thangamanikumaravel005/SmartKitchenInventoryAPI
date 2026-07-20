using SmartKitchenInventoryAPI.DTOs;
using SmartKitchenInventoryAPI.Helpers;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        // Login
        public async Task<string?> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetUserByUsernameAsync(request.Username);

            if (user == null)
            {
                return null;
            }

            bool isPasswordValid = PasswordHasher.VerifyPassword(
                request.Password,
                user.Password);

            if (!isPasswordValid)
            {
                return null;
            }

            return _jwtService.GenerateToken(user);
        }

        // Register
        public async Task<string> RegisterAsync(RegisterRequestDto request)
        {
            // Check whether username already exists
            if (await _userRepository.UserExistsAsync(request.Username))
            {
                return "Username already exists.";
            }

            // Create new user
            var user = new User
            {
                Username = request.Username,
                Password = PasswordHasher.HashPassword(request.Password),
                Role = request.Role,
                IsActive = true
            };

            // Save user
            await _userRepository.AddUserAsync(user);

            return "User registered successfully.";
        }
    }
}
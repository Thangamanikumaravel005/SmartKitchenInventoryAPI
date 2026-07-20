using SmartKitchenInventoryAPI.DTOs;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(LoginRequestDto request);

        Task<string> RegisterAsync(RegisterRequestDto request);
    }
}
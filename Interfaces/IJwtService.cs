using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
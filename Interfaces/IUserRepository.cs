using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsernameAsync(string username);

        Task<bool> UserExistsAsync(string username);

        Task AddUserAsync(User user);
    }
}
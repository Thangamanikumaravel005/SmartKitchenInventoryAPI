using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<Item>> GetAllAsync();

        Task<Item?> GetByIdAsync(int id);

        Task AddAsync(Item item);

        Task UpdateAsync(Item item);

        Task DeleteAsync(Item item);

        Task SaveChangesAsync();
    }
}
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IInventoryService
    {
        Task<IEnumerable<Item>> GetAllItemsAsync();
        Task<IEnumerable<Item>> GetPagedItemsAsync(PaginationParameters paginationParameters);

        Task<Item?> GetItemByIdAsync(int id);

        Task AddItemAsync(Item item);

        Task UpdateItemAsync(Item item);

        Task DeleteItemAsync(int id);
    }
}
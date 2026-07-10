using System.Linq;
using SmartKitchenInventoryAPI.Validators;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;
using SmartKitchenInventoryAPI.Exceptions;

namespace SmartKitchenInventoryAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository _repository;
        private readonly ItemValidator _validator;
        private readonly INotificationService _notificationService;

        public InventoryService(
    IRepository repository,
    ItemValidator validator,
    INotificationService notificationService)
{
    _repository = repository;
    _validator = validator;
    _notificationService = notificationService;
}

        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddItemAsync(Item item)
{
    var items = await _repository.GetAllAsync();

    if (items.Any(i => i.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase)))
    {
        throw new DuplicateItemException($"Item '{item.Name}' already exists.");
    }

    
    if (!_validator.Validate(item, out string errorMessage))
    {
        throw new Exception(errorMessage);
    }

   
    await _repository.AddAsync(item);
    await _repository.SaveChangesAsync();

    
    string notification = _notificationService.CheckNotifications(item);

    Console.WriteLine(notification);
}

        public async Task<IEnumerable<Item>> GetPagedItemsAsync(PaginationParameters parameters)
        {
            var items = await _repository.GetAllAsync();

            if (parameters == null || parameters.PageNumber < 1 || parameters.PageSize < 1)
            {
                return items;
            }

            return items
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var existingItem = await _repository.GetByIdAsync(item.Id);

            if (existingItem == null)
                throw new Exception("Item not found.");

            existingItem.Name = item.Name;
            existingItem.Category = item.Category;
            existingItem.Quantity = item.Quantity;
            existingItem.Unit = item.Unit;
            existingItem.Price = item.Price;
            existingItem.MinimumQuantity = item.MinimumQuantity;
            existingItem.ExpiryDate = item.ExpiryDate;
            existingItem.ModifiedDate = DateTime.Now;

            await _repository.UpdateAsync(existingItem);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(int id)
{
    var item = await _repository.GetByIdAsync(id);

    if (item == null)
    {
        throw new ItemNotFoundException($"Item with Id {id} was not found.");
    }

    await _repository.DeleteAsync(item);
    await _repository.SaveChangesAsync();
}
    }
}
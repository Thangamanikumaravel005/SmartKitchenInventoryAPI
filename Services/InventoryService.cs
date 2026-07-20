using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using SmartKitchenInventoryAPI.Exceptions;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IRepository _repository;
        private readonly ItemValidator _validator;
        private readonly INotificationService _notificationService;
        private readonly IMemoryCache _cache;
        private readonly ILogger<InventoryService> _logger;

        private const string CacheKey = "InventoryItems";

        public InventoryService(
            IRepository repository,
            ItemValidator validator,
            INotificationService notificationService,
            IMemoryCache cache,
            ILogger<InventoryService> logger)
        {
            _repository = repository;
            _validator = validator;
            _notificationService = notificationService;
            _cache = cache;
            _logger = logger;
        }

        // Get all items using Memory Cache
        public async Task<IEnumerable<Item>> GetAllItemsAsync()
        {
            _logger.LogInformation("Entered GetAllItemsAsync");

            if (!_cache.TryGetValue(CacheKey, out IEnumerable<Item>? items))
            {
                _logger.LogInformation("Cache miss. Loading inventory items from database.");

                items = await _repository.GetAllAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                _cache.Set(CacheKey, items, cacheOptions);

                _logger.LogInformation("Inventory items stored in cache for 5 minutes.");
            }
            else
            {
                _logger.LogInformation("Cache hit. Returning inventory items from memory.");
            }

            return items!;
        }

        // Get paged items using cached data
        public async Task<IEnumerable<Item>> GetPagedItemsAsync(PaginationParameters parameters)
        {
            _logger.LogInformation("Entered GetPagedItemsAsync");

            var items = await GetAllItemsAsync();

            if (parameters == null || parameters.PageNumber < 1 || parameters.PageSize < 1)
            {
                return items;
            }

            return items
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize);
        }

        public async Task<Item?> GetItemByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddItemAsync(Item item)
        {
            var items = await GetAllItemsAsync();

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

            _cache.Remove(CacheKey);
            _logger.LogInformation("Inventory cache cleared after adding an item.");

            string notification = _notificationService.CheckNotifications(item);

            Console.WriteLine(notification);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var existingItem = await _repository.GetByIdAsync(item.Id);

            if (existingItem == null)
            {
                throw new Exception("Item not found.");
            }

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

            _cache.Remove(CacheKey);
            _logger.LogInformation("Inventory cache cleared after updating an item.");
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

            _cache.Remove(CacheKey);
            _logger.LogInformation("Inventory cache cleared after deleting an item.");
        }
    }
}
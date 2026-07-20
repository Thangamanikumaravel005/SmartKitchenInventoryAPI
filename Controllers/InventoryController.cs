using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartKitchenInventoryAPI.Helpers;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(
            IInventoryService inventoryService,
            ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        // GET: api/inventory/paged
        [HttpGet("paged")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetPagedItems([FromQuery] PaginationParameters parameters)
        {
            _logger.LogInformation(
                "Pagination requested. PageNumber: {PageNumber}, PageSize: {PageSize}",
                parameters.PageNumber,
                parameters.PageSize);

            var items = await _inventoryService.GetPagedItemsAsync(parameters);

            _logger.LogInformation("Returned {Count} items.", items.Count());

            return Ok(items);
        }

        // GET: api/inventory/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> GetItemById(int id)
        {
            _logger.LogInformation("Getting inventory item with Id: {Id}", id);

            var item = await _inventoryService.GetItemByIdAsync(id);

            if (item == null)
            {
                _logger.LogWarning("Inventory item with Id {Id} not found.", id);

                return NotFound(new ApiResponse<string>(
                    false,
                    $"Item with Id {id} not found.",
                    null));
            }

            _logger.LogInformation("Inventory item retrieved successfully.");

            return Ok(new ApiResponse<Item>(
                true,
                "Item retrieved successfully.",
                item));
        }

        // POST: api/inventory
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddItem(Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding new inventory item: {Name}", item.Name);

            await _inventoryService.AddItemAsync(item);

            _logger.LogInformation("Inventory item added successfully.");

            return Ok(new ApiResponse<string>(
                true,
                "Item added successfully.",
                null));
        }

        // PUT: api/inventory/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.Id)
            {
                return BadRequest(new ApiResponse<string>(
                    false,
                    "Id mismatch.",
                    null));
            }

            _logger.LogInformation("Updating inventory item with Id: {Id}", id);

            await _inventoryService.UpdateItemAsync(item);

            _logger.LogInformation("Inventory item updated successfully.");

            return Ok(new ApiResponse<string>(
                true,
                "Item updated successfully.",
                null));
        }

        // DELETE: api/inventory/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            _logger.LogInformation("Deleting inventory item with Id: {Id}", id);

            await _inventoryService.DeleteItemAsync(id);

            _logger.LogInformation("Inventory item deleted successfully.");

            return Ok(new ApiResponse<string>(
                true,
                "Item deleted successfully.",
                null));
        }
    }
}
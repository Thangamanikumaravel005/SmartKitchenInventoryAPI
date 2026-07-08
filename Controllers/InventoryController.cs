using Microsoft.AspNetCore.Mvc;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;
using SmartKitchenInventoryAPI.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace SmartKitchenInventoryAPI.Controllers
{
   [ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        
        [HttpGet]
public async Task<IActionResult> GetAllItems()
{
    var items = await _inventoryService.GetAllItemsAsync();

    var response = new ApiResponse<IEnumerable<Item>>(
        true,
        "Items retrieved successfully.",
        items);

    return Ok(response);
}

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            var item = await _inventoryService.GetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound(new
                {
                    Message = $"Item with Id {id} not found."
                });
            }

           return Ok(
    new ApiResponse<Item>(
        true,
        "Item retrieved successfully.",
        item));
        }

        
        [HttpPost]
[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddItem(Item item)
        {
            await _inventoryService.AddItemAsync(item);

            return Ok(
    new ApiResponse<string>(
        true,
        "Item added successfully.",
        null));
        }

        
        [HttpPut("{id}")]
[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateItem(int id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest(new
                {
                    Message = "Id mismatch."
                });
            }

            await _inventoryService.UpdateItemAsync(item);

            return Ok(
    new ApiResponse<string>(
        true,
        "Item updated successfully.",
        null));
        }

        
       [HttpDelete("{id}")]
[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            await _inventoryService.DeleteItemAsync(id);

           return Ok(
    new ApiResponse<string>(
        true,
        "Item deleted successfully.",
        null));
        }
    }
}
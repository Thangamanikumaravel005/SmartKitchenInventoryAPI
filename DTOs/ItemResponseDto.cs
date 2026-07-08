using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.DTOs
{
    public class ItemResponseDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Category Category { get; set; }

        public int Quantity { get; set; }

        public Unit Unit { get; set; }

        public decimal Price { get; set; }

        public int MinimumQuantity { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}
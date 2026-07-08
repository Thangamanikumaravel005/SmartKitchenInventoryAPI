using System.ComponentModel.DataAnnotations;

namespace SmartKitchenInventoryAPI.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Category Category { get; set; }

        [Required]
        public double Quantity { get; set; }

        [Required]
        public Unit Unit { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public double MinimumQuantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime? ModifiedDate { get; set; }
    }
}
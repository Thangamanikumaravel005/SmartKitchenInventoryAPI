using System.ComponentModel.DataAnnotations;

public class Item
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Item name is required.")]
    [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters.")]
    public string Name { get; set; } = String.Empty;

    [Required(ErrorMessage = "Category is required.")]
    public string Category { get; set; } = String.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Unit is required.")]
    public string Unit { get; set; } = String.Empty;

    [Range(0.01, 100000, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Minimum quantity cannot be negative.")]
    public int MinimumQuantity { get; set; }

    [Required(ErrorMessage = "Expiry date is required.")]
    public DateTime ExpiryDate { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public DateTime? DeletedDate { get; set; }
}
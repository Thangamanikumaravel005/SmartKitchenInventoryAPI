using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Validators
{
    public class ItemValidator
    {
        public bool Validate(Item item, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                errorMessage = "Item name is required.";
                return false;
            }

            if (item.Quantity < 0)
            {
                errorMessage = "Quantity cannot be negative.";
                return false;
            }

            if (item.Price <= 0)
            {
                errorMessage = "Price must be greater than zero.";
                return false;
            }

            if (item.MinimumQuantity < 0)
            {
                errorMessage = "Minimum quantity cannot be negative.";
                return false;
            }

            if (item.ExpiryDate < DateTime.Today)
            {
                errorMessage = "Expiry date cannot be in the past.";
                return false;
            }

            return true;
        }
    }
}
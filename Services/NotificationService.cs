using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Services
{
    public class NotificationService : INotificationService
    {
        public string CheckNotifications(Item item)
        {
            if (item.Quantity <= item.MinimumQuantity)
            {
                return $"⚠️ Low Stock Alert: {item.Name} is below the minimum quantity.";
            }

            if (item.ExpiryDate <= DateTime.Today)
            {
                return $"❌ {item.Name} has expired.";
            }

            if (item.ExpiryDate <= DateTime.Today.AddDays(7))
            {
                return $"📅 {item.Name} will expire within 7 days.";
            }

            return "No notifications.";
        }
    }
}
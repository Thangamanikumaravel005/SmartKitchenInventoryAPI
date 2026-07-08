using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface INotificationService
    {
        string CheckNotifications(Item item);
    }
}
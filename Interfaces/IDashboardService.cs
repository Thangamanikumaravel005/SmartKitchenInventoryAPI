using SmartKitchenInventoryAPI.DTOs;

namespace SmartKitchenInventoryAPI.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardDataAsync();
    }
}
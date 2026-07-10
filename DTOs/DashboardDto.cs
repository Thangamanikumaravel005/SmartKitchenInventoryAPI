namespace SmartKitchenInventoryAPI.DTOs
{
    public class DashboardDto
    {
        public int TotalItems { get; set; }

        public int LowStockItems { get; set; }

        public int ExpiringItems { get; set; }

        public decimal TotalInventoryValue { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using SmartKitchenInventoryAPI.Data;
using SmartKitchenInventoryAPI.DTOs;
using SmartKitchenInventoryAPI.Interfaces;

namespace SmartKitchenInventoryAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardDto> GetDashboardDataAsync()
        {
            var items = await _context.Items
                .Where(i => !i.IsDeleted)
                .ToListAsync();

            var dashboard = new DashboardDto
            {
                TotalItems = items.Count,

                LowStockItems = items.Count(i => i.Quantity <= i.MinimumQuantity),

                ExpiringItems = items.Count(i =>
                    i.ExpiryDate <= DateTime.Today.AddDays(7)),

                TotalInventoryValue = items.Sum(i => (decimal)i.Quantity * i.Price)
            };

            return dashboard;
        }
    }
}
using Microsoft.EntityFrameworkCore;
using SmartKitchenInventoryAPI.Data;
using SmartKitchenInventoryAPI.Interfaces;
using SmartKitchenInventoryAPI.Models;

namespace SmartKitchenInventoryAPI.Repositories
{
    public class InventoryRepository : IRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Item>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Item?> GetByIdAsync(int id)
        {
            return await _context.Items.FindAsync(id);
        }

        public async Task AddAsync(Item item)
        {
            await _context.Items.AddAsync(item);
        }

        public Task UpdateAsync(Item item)
        {
            _context.Items.Update(item);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Item item)
        {
            _context.Items.Remove(item);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace flexa.Infra.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly AppDbContext _context;

        public ScreenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync(string userId)
        {
            return await _context.Screens
                .Include(s => s.Components)
                .Include(s => s.DataEntries)
                .Where(s => s.AppEntity.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int id, string userId)
        {
            return await _context.Screens
                .Include(s => s.Components)
                .Include(s => s.DataEntries)
                .FirstOrDefaultAsync(s => s.Id == id && s.AppEntity.UserId == userId);
        }

        public async Task<Screen> AddAsync(Screen entity, string userId)
        {
            // Verify the AppEntityId belongs to the user
            var appEntity = await _context.AppEntities
                .FirstOrDefaultAsync(a => a.Id == entity.AppEntityId && a.UserId == userId)
                ?? throw new KeyNotFoundException("AppEntity not found for this user.");

            entity.Id = 0; // Ensure EF Core generates a new ID
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UserId = userId;

            _context.Screens.Add(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task UpdateAsync(Screen entity, string userId)
        {
            var existing = await _context.Screens
                .Include(s => s.Components)
                .Include(s => s.DataEntries)
                .FirstOrDefaultAsync(s => s.Id == entity.Id && s.AppEntity.UserId == userId)
                ?? throw new KeyNotFoundException("Screen not found for this user.");

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id, string userId)
        {
            var entity = await _context.Screens
                .FirstOrDefaultAsync(s => s.Id == id && s.AppEntity.UserId == userId);

            if (entity != null)
            {
                _context.Screens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

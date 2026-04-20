using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly AppDbContext _context;

        public ScreenRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Screen>> GetAllAsync()
        {
            return await _context.Screens
                .Include(s => s.Components)
                .Include(s => s.DataEntries)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(Guid id)
        {
            return await _context.Screens
                .Include(s => s.Components)
                .Include(s => s.DataEntries)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<Screen> AddAsync(Screen entity)
        {
            entity.Id = Guid.NewGuid();
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.Screens.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Screen> UpdateAsync(Screen entity)
        {
            var existing = await _context.Screens.FindAsync(entity.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Screen with Id {entity.Id} not found.");

            existing.Name = entity.Name;
            existing.Description = entity.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            _context.Screens.Update(existing);
            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Screens.FindAsync(id);
            if (entity != null)
            {
                _context.Screens.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}

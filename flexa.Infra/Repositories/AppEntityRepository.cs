using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories
{
    public class AppEntityRepository(AppDbContext context) : IAppEntityRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<AppEntity>> GetAllAsync(string userId)
        {
            return await _context.AppEntities
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<AppEntity?> GetByIdAsync(Guid id, string userId)
        {
            return await _context.AppEntities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
        }

        public async Task<AppEntity?> GetByPublicLinkAsync(string publicLink)
        {
            return await _context.AppEntities
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.PublicLink == publicLink);
        }

        public async Task AddAsync(AppEntity app)
        {
            await _context.AppEntities.AddAsync(app);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppEntity app)
        {
            var existing = await _context.AppEntities
               .FirstOrDefaultAsync(a => a.Id == app.Id && a.UserId == app.UserId)
               ?? throw new KeyNotFoundException($"AppEntity {app.Id} não encontrado para este usuário.");

            existing.Name = app.Name;
            existing.Description = app.Description;
            existing.PublicLink = app.PublicLink;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            var app = await _context.AppEntities
                .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);
            if (app != null)
            {
                _context.AppEntities.Remove(app);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories
{
    public class AppEntityRepository : IAppEntityRepository
    {
        private readonly AppDbContext _context;

        public AppEntityRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AppEntity>> GetAllAsync()
        {
            return await _context.AppEntities.ToListAsync();
        }

        public async Task<AppEntity?> GetByIdAsync(Guid id)
        {
            return await _context.AppEntities.FindAsync(id);
        }

        public async Task AddAsync(AppEntity app)
        {
            await _context.AppEntities.AddAsync(app);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AppEntity app)
        {
            _context.AppEntities.Update(app);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var app = await _context.AppEntities.FindAsync(id);
            if (app != null)
            {
                _context.AppEntities.Remove(app);
                await _context.SaveChangesAsync();
            }
        }
    }
}
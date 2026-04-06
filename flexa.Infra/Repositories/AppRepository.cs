using flexa.Domain.Interfaces;
using flexa.Domain.Entities;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace flexa.Infra.Repositories
{
    public class AppRepository : IAppRepository
    {
        private readonly AppDbContext _context;

        public AppRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<App>> GetAllAsync()
        {
            return await _context.Apps.ToListAsync();
        }

        public async Task<App?> GetByIdAsync(Guid id)
        {
            return await _context.Apps.FindAsync(id);
        }

        public async Task AddAsync(App app)
        {
            await _context.Apps.AddAsync(app);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(App app)
        {
            _context.Apps.Update(app);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var app = await _context.Apps.FindAsync(id);
            if (app != null)
            {
                _context.Apps.Remove(app);
                await _context.SaveChangesAsync();
            }
        }
    }
}
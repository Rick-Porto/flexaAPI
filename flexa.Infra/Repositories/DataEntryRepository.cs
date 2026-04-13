using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories;

public class DataEntryRepository(AppDbContext context) : IDataEntryRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<DataEntry>> GetAllAsync() =>
        await _context.DataEntries.AsNoTracking().ToListAsync();

    public async Task<DataEntry?> GetByIdAsync(Guid id) =>
        await _context.DataEntries.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DataEntry>> GetByScreenIdAsync(Guid screenId) =>
        await _context.DataEntries
            .Where(d => d.ScreenId == screenId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<DataEntry>> GetByComponentIdAsync(Guid componentId) =>
        await _context.DataEntries
            .Where(d => d.ComponentId == componentId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<DataEntry> AddAsync(DataEntry entity)
    {
        await _context.DataEntries.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<DataEntry> UpdateAsync(DataEntry entity)
    {
        var existing = await _context.DataEntries.FindAsync(entity.Id)
            ?? throw new KeyNotFoundException($"DataEntry {entity.Id} not found.");

        existing.Config = entity.Config;
        existing.Data = entity.Data;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.DataEntries.FindAsync(id);
        if (entity is null) return false;
        _context.DataEntries.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
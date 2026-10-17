using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories;

public class DataEntryRepository(AppDbContext context) : IDataEntryRepository
{
    private readonly AppDbContext _context = context;

    // ✅ JOIN DataEntry → Screen → AppEntity.UserId
    public async Task<IEnumerable<DataEntry>> GetAllAsync(string userId) =>
        await _context.DataEntries
            .Where(d => d.Screen!.AppEntity.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<DataEntry?> GetByIdAsync(int id, string userId) =>
        await _context.DataEntries
            .Where(d => d.Id == id && d.Screen!.AppEntity.UserId == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

    public async Task<IEnumerable<DataEntry>> GetByScreenIdAsync(int screenId, string userId) =>
        await _context.DataEntries
            .Where(d => d.ScreenId == screenId && d.Screen!.AppEntity.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<IEnumerable<DataEntry>> GetByComponentIdAsync(int componentId, string userId) =>
        await _context.DataEntries
            .Where(d => d.ComponentId == componentId && d.Screen!.AppEntity.UserId == userId)
            .AsNoTracking()
            .ToListAsync();

    // ✅ Verifica ownership do ScreenId e ComponentId antes de inserir
    public async Task<DataEntry> AddAsync(DataEntry entity, string userId)
    {
        if (string.IsNullOrWhiteSpace(entity.UserId))
            entity.UserId = userId;

        if (entity.CreatedAt == default)
            entity.CreatedAt = DateTime.UtcNow;

        if (entity.UpdatedAt == default)
            entity.UpdatedAt = DateTime.UtcNow;

        var screenValid = await _context.Screens
            .AnyAsync(s => s.Id == entity.ScreenId && s.AppEntity.UserId == userId);
        if (!screenValid)
            throw new KeyNotFoundException("Screen não encontrado para este usuário.");

        var componentValid = await _context.Components
            .AnyAsync(c => c.Id == entity.ComponentId && c.Screen.AppEntity.UserId == userId);
        if (!componentValid)
            throw new KeyNotFoundException("Component não encontrado para este usuário.");

        await _context.DataEntries.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    // ✅ Só atualiza se pertence ao usuário
    public async Task<DataEntry?> UpdateAsync(DataEntry entity, string userId)
    {
        var existing = await _context.DataEntries
            .FirstOrDefaultAsync(d => d.Id == entity.Id && d.Screen!.AppEntity.UserId == userId);

        if (existing is null) return null;

        existing.Config    = entity.Config;
        existing.Data      = entity.Data;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    // ✅ Só deleta se pertence ao usuário
    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var entity = await _context.DataEntries
            .FirstOrDefaultAsync(d => d.Id == id && d.Screen!.AppEntity.UserId == userId);

        if (entity is null) return false;

        _context.DataEntries.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}

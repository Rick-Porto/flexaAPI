using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories;

public class ComponentRepository : IComponentRepository
{
    private readonly AppDbContext _context;

    public ComponentRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Component>> GetAllAsync()
    {
        return await _context.Components
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Component?> GetByIdAsync(Guid id)
    {
        return await _context.Components
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Component>> GetComponentsByScreenIdAsync(Guid screenId)
    {
        return await _context.Components
            .Where(c => c.ScreenId == screenId)
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Component> AddAsync(Component component)
    {
        await _context.Components.AddAsync(component);
        await _context.SaveChangesAsync();
        return component;
    }

    public async Task<Component> UpdateAsync(Component component)
    {
        var existing = await _context.Components.FindAsync(component.Id)
            ?? throw new KeyNotFoundException($"Component {component.Id} not found.");

        existing.ElementType = component.ElementType;
        existing.Label = component.Label;
        existing.Model = component.Model;
        existing.Config = component.Config;
        existing.Order = component.Order;
        existing.Required = component.Required;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Components.FindAsync(id);
        if (entity is null) return false;
        _context.Components.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace flexa.Infra.Repositories;

public class ComponentRepository(AppDbContext context) : IComponentRepository
{
    private readonly AppDbContext _context = context;

    // JOIN Component → Screen → AppEntity.UserId
    public async Task<IEnumerable<Component>> GetAllAsync(string userId)
    {
        return await _context.Components
            .Where(c => c.Screen.AppEntity.UserId == userId)
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .ToListAsync();
    }

    // AND no UserId — impede user A de ler component de user B pelo GUID
    public async Task<Component?> GetByIdAsync(int id, string userId)
    {
        return await _context.Components
            .Where(c => c.Id == id && c.Screen.AppEntity.UserId == userId)
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    // Só retorna components de screens que pertencem ao usuário
    public async Task<IEnumerable<Component>> GetComponentsByScreenIdAsync(int screenId, string userId)
    {
        return await _context.Components
            .Where(c => c.ScreenId == screenId && c.Screen.AppEntity.UserId == userId)
            .Include(c => c.DataEntries)
            .AsNoTracking()
            .ToListAsync();
    }

    // Verifica que o ScreenId informado pertence ao usuário antes de inserir
    public async Task<Component> AddAsync(Component component, string userId)
    {
        var screenExists = await _context.Screens
            .AnyAsync(s => s.Id == component.ScreenId && s.AppEntity.UserId == userId);

        if (!screenExists)
            throw new KeyNotFoundException("Screen não encontrado para este usuário.");

        await _context.Components.AddAsync(component);
        await _context.SaveChangesAsync();
        return component;
    }

    // Só atualiza se o component pertence ao usuário (via Screen → AppEntity)
    public async Task<Component?> UpdateAsync(Component component, string userId)
    {
        var existing = await _context.Components
            .FirstOrDefaultAsync(c => c.Id == component.Id && c.Screen.AppEntity.UserId == userId);

        if (existing is null) return null;

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

    // Só deleta se o component pertence ao usuário
    public async Task<bool> DeleteAsync(int id, string userId)
    {
        var entity = await _context.Components
            .FirstOrDefaultAsync(c => c.Id == id && c.Screen.AppEntity.UserId == userId);

        if (entity is null) return false;

        _context.Components.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
using flexa.App.DTOs.Component;
using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services;

public class ComponentService : IComponentService
{
    private readonly IComponentRepository _repo;

    public ComponentService(IComponentRepository repo) => _repo = repo;

    public async Task<IEnumerable<ComponentResponse>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<ComponentResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<ComponentResponse>> GetByScreenIdAsync(Guid screenId)
    {
        var entities = await _repo.GetComponentsByScreenIdAsync(screenId);
        return entities.Select(MapToResponse);
    }

    public async Task<ComponentResponse> CreateAsync(ComponentRequest request)
    {
        var entity = new Component
        {
            Id = Guid.NewGuid(),
            ElementType = request.ElementType,
            Label = request.Label,
            Model = request.Model,
            Config = request.Config,
            Order = request.Order,
            Required = request.Required,
            ScreenId = request.ScreenId,
            Screen = null!,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(entity);
        return MapToResponse(created);
    }

    public async Task<ComponentResponse?> UpdateAsync(Guid id, ComponentRequest request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return null;

        existing.ElementType = request.ElementType;
        existing.Label = request.Label;
        existing.Model = request.Model;
        existing.Config = request.Config;
        existing.Order = request.Order;
        existing.Required = request.Required;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _repo.UpdateAsync(existing);
        return MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

    private static ComponentResponse MapToResponse(Component c) => new()
    {
        Id = c.Id,
        ElementType = c.ElementType,
        Label = c.Label,
        Model = c.Model,
        Config = c.Config,
        Order = c.Order,
        Required = c.Required,
        ScreenId = c.ScreenId,
        CreatedAt = c.CreatedAt,
        UpdatedAt = c.UpdatedAt,
        DataEntries = c.DataEntries?.Select(de => new DataEntryResponse
        {
            Id = de.Id,
            Config = de.Config,
            Data = de.Data,
            CreatedAt = de.CreatedAt,
            UpdatedAt = de.UpdatedAt,
            ScreenId = de.ScreenId,
            ComponentId = de.ComponentId
        }).ToList()
    };
}
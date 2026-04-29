using flexa.App.DTOs.Component;
using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services;

public class ComponentService(IComponentRepository repo) : IComponentService
{
    private readonly IComponentRepository _repo = repo;

    public async Task<IEnumerable<ComponentResponse>> GetAllAsync(string userId)
    {
        var entities = await _repo.GetAllAsync(userId);
        return entities.Select(MapToResponse);
    }

    public async Task<ComponentResponse?> GetByIdAsync(int id, string userId)
    {
        var entity = await _repo.GetByIdAsync(id, userId);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<ComponentResponse>> GetByScreenIdAsync(int screenId, string userId)
    {
        var entities = await _repo.GetComponentsByScreenIdAsync(screenId, userId);
        return entities.Select(MapToResponse);
    }

    public async Task<ComponentResponse> CreateAsync(ComponentRequest request, string userId)
    {
        var entity = new Component
        {
            ElementType = request.ElementType,
            Label = request.Label,
            Model = request.Model,
            Config = request.Config,
            Order = request.Order,
            Required = request.Required,
            ScreenId = request.ScreenId,
            Screen = null!
        };

        // repositório valida o ScreenId e seta Id, CreatedAt, UpdatedAt
        var created = await _repo.AddAsync(entity, userId);
        return MapToResponse(created);
    }

    public async Task<ComponentResponse?> UpdateAsync(int id, ComponentRequest request, string userId)
    {
        var entity = new Component
        {
            Id = id,
            ElementType = request.ElementType,
            Label = request.Label,
            Model = request.Model,
            Config = request.Config,
            Order = request.Order,
            Required = request.Required,
            ScreenId = request.ScreenId,
            Screen = null!
        };

        var updated = await _repo.UpdateAsync(entity, userId);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
        => await _repo.DeleteAsync(id, userId);

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
            ScreenId = de.ScreenId,
            ComponentId = de.ComponentId,
            CreatedAt = de.CreatedAt,
            UpdatedAt = de.UpdatedAt
        }).ToList()
    };
}
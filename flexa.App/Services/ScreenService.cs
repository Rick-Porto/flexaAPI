using flexa.App.DTOs.Screen;
using flexa.App.DTOs.Component;
using flexa.App.DTOs.DataEntry;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;
using flexa.App.Interfaces;

namespace flexa.App.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _repo;

    public ScreenService(IScreenRepository repo)
    {
        _repo = repo;
    }

    private static ScreenResponse MapToResponse(Screen s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        Description = s.Description,
        AppEntityId = s.AppEntityId,
        CreatedAt = s.CreatedAt,
        UpdatedAt = s.UpdatedAt,
        Components = s.Components?.Select(c => new ComponentResponse
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
            UpdatedAt = c.UpdatedAt
        }).ToList(),
        DataEntries = s.DataEntries?.Select(de => new DataEntryResponse
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

    public async Task<IEnumerable<ScreenResponse>> GetAllAsync(string userId)
    {
        var entities = await _repo.GetAllAsync(userId);
        return entities.Select(MapToResponse);
    }

    public async Task<ScreenResponse?> GetByIdAsync(int id, string userId)
    {
        var entity = await _repo.GetByIdAsync(id, userId);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<ScreenResponse> AddAsync(ScreenRequest request, string userId)
    {
        var entity = new Screen
        {
            Name = request.Name,
            Description = request.Description,
            AppEntityId = request.AppEntityId,
            AppEntity = null!,
            Components = null!,
            DataEntries = null!
        };

        var created = await _repo.AddAsync(entity, userId);
        return MapToResponse(created);
    }

    public async Task UpdateAsync(int id, ScreenRequest request, string userId)
    {
        var entity = new Screen
        {
            Id = id,
            Name = request.Name,
            Description = request.Description,
            AppEntityId = request.AppEntityId,
            AppEntity = null!
        };

        await _repo.UpdateAsync(entity, userId);
    }

    public async Task DeleteAsync(int id, string userId)
        => await _repo.DeleteAsync(id, userId);
}

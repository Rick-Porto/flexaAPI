using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services;

public class DataEntryService(IDataEntryRepository repo) : IDataEntryService
{
    private readonly IDataEntryRepository _repo = repo;

    public async Task<IEnumerable<DataEntryResponse>> GetAllAsync()
    {
        var entities = await _repo.GetAllAsync();
        return entities.Select(MapToResponse);
    }

    public async Task<DataEntryResponse?> GetByIdAsync(Guid id)
    {
        var entity = await _repo.GetByIdAsync(id);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<DataEntryResponse>> GetByScreenIdAsync(Guid screenId)
    {
        var entities = await _repo.GetByScreenIdAsync(screenId);
        return entities.Select(MapToResponse);
    }

    public async Task<IEnumerable<DataEntryResponse>> GetByComponentIdAsync(Guid componentId)
    {
        var entities = await _repo.GetByComponentIdAsync(componentId);
        return entities.Select(MapToResponse);
    }

    public async Task<DataEntryResponse> CreateAsync(DataEntryRequest request)
    {
        var entity = new DataEntry
        {
            Id = Guid.NewGuid(),
            Config = request.Config,
            Data = request.Data,
            ScreenId = request.ScreenId,
            ComponentId = request.ComponentId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var created = await _repo.AddAsync(entity);
        return MapToResponse(created);
    }

    public async Task<DataEntryResponse?> UpdateAsync(Guid id, DataEntryRequest request)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing is null) return null;

        existing.Config = request.Config;
        existing.Data = request.Data;
        existing.UpdatedAt = DateTime.UtcNow;

        var updated = await _repo.UpdateAsync(existing);
        return MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(Guid id) => await _repo.DeleteAsync(id);

    private static DataEntryResponse MapToResponse(DataEntry d) => new()
    {
        Id = d.Id,
        Config = d.Config,
        Data = d.Data,
        ScreenId = d.ScreenId,
        ComponentId = d.ComponentId,
        CreatedAt = d.CreatedAt,
        UpdatedAt = d.UpdatedAt
    };
}
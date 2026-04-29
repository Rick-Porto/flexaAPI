using flexa.App.DTOs.DataEntry;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services;

public class DataEntryService(IDataEntryRepository repo) : IDataEntryService
{
    private readonly IDataEntryRepository _repo = repo;

    public async Task<IEnumerable<DataEntryResponse>> GetAllAsync(string userId)
    {
        var entities = await _repo.GetAllAsync(userId);
        return entities.Select(MapToResponse);
    }

    public async Task<DataEntryResponse?> GetByIdAsync(int id, string userId)
    {
        var entity = await _repo.GetByIdAsync(id, userId);
        return entity is null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<DataEntryResponse>> GetByScreenIdAsync(int screenId, string userId)
    {
        var entities = await _repo.GetByScreenIdAsync(screenId, userId);
        return entities.Select(MapToResponse);
    }

    public async Task<IEnumerable<DataEntryResponse>> GetByComponentIdAsync(int componentId, string userId)
    {
        var entities = await _repo.GetByComponentIdAsync(componentId, userId);
        return entities.Select(MapToResponse);
    }

    public async Task<DataEntryResponse> CreateAsync(DataEntryRequest request, string userId)
    {
        var entity = new DataEntry
        {
            Config      = request.Config,
            Data        = request.Data,
            ScreenId    = request.ScreenId,
            ComponentId = request.ComponentId
        };

        // repositório valida ownership e seta Id, CreatedAt, UpdatedAt
        var created = await _repo.AddAsync(entity, userId);
        return MapToResponse(created);
    }

    public async Task<DataEntryResponse?> UpdateAsync(int id, DataEntryRequest request, string userId)
    {
        var entity = new DataEntry
        {
            Id     = id,
            Config = request.Config,
            Data   = request.Data
        };

        var updated = await _repo.UpdateAsync(entity, userId);
        return updated is null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int id, string userId)
        => await _repo.DeleteAsync(id, userId);

    private static DataEntryResponse MapToResponse(DataEntry d) => new()
    {
        Id          = d.Id,
        Config      = d.Config,
        Data        = d.Data,
        ScreenId    = d.ScreenId,
        ComponentId = d.ComponentId,
        CreatedAt   = d.CreatedAt,
        UpdatedAt   = d.UpdatedAt
    };
}

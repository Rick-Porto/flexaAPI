using flexa.App.DTOs.DataEntry;

namespace flexa.App.Interfaces;

public interface IDataEntryService
{
    Task<IEnumerable<DataEntryResponse>> GetAllAsync();
    Task<DataEntryResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<DataEntryResponse>> GetByScreenIdAsync(Guid screenId);
    Task<IEnumerable<DataEntryResponse>> GetByComponentIdAsync(Guid componentId);
    Task<DataEntryResponse> CreateAsync(DataEntryRequest request);
    Task<DataEntryResponse?> UpdateAsync(Guid id, DataEntryRequest request);
    Task<bool> DeleteAsync(Guid id);
}
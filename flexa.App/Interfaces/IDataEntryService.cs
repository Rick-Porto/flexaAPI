using flexa.App.DTOs.DataEntry;

namespace flexa.App.Interfaces;

public interface IDataEntryService
{
    Task<IEnumerable<DataEntryResponse>> GetAllAsync(string userId);
    Task<DataEntryResponse?> GetByIdAsync(int id, string userId);
    Task<IEnumerable<DataEntryResponse>> GetByScreenIdAsync(int screenId, string userId);
    Task<IEnumerable<DataEntryResponse>> GetByComponentIdAsync(int componentId, string userId);
    Task<DataEntryResponse> CreateAsync(DataEntryRequest request, string userId);
    Task<DataEntryResponse?> UpdateAsync(int id, DataEntryRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}

using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces;

public interface IDataEntryRepository
{
    Task<IEnumerable<DataEntry>> GetAllAsync(string userId);
    Task<DataEntry?> GetByIdAsync(int id, string userId);
    Task<IEnumerable<DataEntry>> GetByScreenIdAsync(int screenId, string userId);
    Task<IEnumerable<DataEntry>> GetByComponentIdAsync(int componentId, string userId);
    Task<DataEntry> AddAsync(DataEntry entity, string userId);
    Task<DataEntry?> UpdateAsync(DataEntry entity, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}

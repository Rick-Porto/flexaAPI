using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces;

public interface IDataEntryRepository
{
    Task<IEnumerable<DataEntry>> GetAllAsync();
    Task<DataEntry?> GetByIdAsync(Guid id);
    Task<IEnumerable<DataEntry>> GetByScreenIdAsync(Guid screenId);
    Task<IEnumerable<DataEntry>> GetByComponentIdAsync(Guid componentId);
    Task<DataEntry> AddAsync(DataEntry entity);
    Task<DataEntry> UpdateAsync(DataEntry entity);
    Task<bool> DeleteAsync(Guid id);
}
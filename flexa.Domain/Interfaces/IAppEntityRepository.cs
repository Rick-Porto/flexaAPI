using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces
{
    public interface IAppEntityRepository
    {
        Task<IEnumerable<AppEntity>> GetAllAsync();
        Task<AppEntity?> GetByIdAsync(Guid id);
        Task AddAsync(AppEntity app);
        Task UpdateAsync(AppEntity app);
        Task DeleteAsync(Guid id);
    }
}
using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces
{
    public interface IAppEntityRepository
    {
        Task<IEnumerable<AppEntity>> GetAllAsync(string userId);
        Task<AppEntity?> GetByIdAsync(Guid id, string userId);
        Task AddAsync(AppEntity app);
        Task UpdateAsync(AppEntity app);
        Task DeleteAsync(Guid id, string userId);
    }
}
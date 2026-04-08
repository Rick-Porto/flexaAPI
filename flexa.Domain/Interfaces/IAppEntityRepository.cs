using flexa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
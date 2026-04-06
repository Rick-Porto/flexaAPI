using flexa.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace flexa.Domain.Interfaces
{
    public interface IAppRepository
    {
        Task<IEnumerable<App>> GetAllAsync();
        Task<App?> GetByIdAsync(Guid id);
        Task AddAsync(App app);
        Task UpdateAsync(App app);
        Task DeleteAsync(Guid id);
    }
}
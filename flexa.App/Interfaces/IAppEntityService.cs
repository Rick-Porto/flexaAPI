using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using flexa.App.DTOs.AppEntity;

namespace flexa.App.Interfaces
{
    public interface IAppEntityService
    {
        Task<IEnumerable<AppEntityResponse>> GetAllAsync();
        Task<AppEntityResponse> GetByIdAsync(Guid id);
        Task<AppEntityResponse> AddAsync(AppEntityRequest appRequest);
        Task UpdateAsync(Guid id, AppEntityRequest appRequest);
        Task DeleteAsync(Guid id);
    }
}
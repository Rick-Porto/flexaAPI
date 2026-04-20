using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using flexa.App.DTOs.Screen;

namespace flexa.App.Interfaces
{
    public interface IScreenService
    {
        Task<IEnumerable<ScreenResponse>> GetAllAsync();
        Task<ScreenResponse?> GetByIdAsync(Guid id);
        Task<ScreenResponse> CreateAsync(ScreenRequest request);
        Task<ScreenResponse> UpdateAsync(Guid id, ScreenRequest request);
        Task DeleteAsync(Guid id);
    }
}

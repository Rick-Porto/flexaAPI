using flexa.App.DTOs.Screen;
using flexa.Domain.Entities;

namespace flexa.App.Interfaces;

public interface IScreenService
{
    Task<IEnumerable<ScreenResponse>> GetAllAsync(string userId);
    Task<ScreenResponse?> GetByIdAsync(int id, string userId);
    Task<ScreenResponse> AddAsync(ScreenRequest request, string userId);
    Task UpdateAsync(int id, ScreenRequest request, string userId);
    Task DeleteAsync(int id, string userId);
}
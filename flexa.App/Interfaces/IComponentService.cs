using flexa.App.DTOs.Component;

namespace flexa.App.Interfaces;

public interface IComponentService
{
    Task<IEnumerable<ComponentResponse>> GetAllAsync(string userId);
    Task<ComponentResponse?> GetByIdAsync(int id, string userId);
    Task<IEnumerable<ComponentResponse>> GetByScreenIdAsync(int screenId, string userId);
    Task<ComponentResponse> CreateAsync(ComponentRequest request, string userId);
    Task<ComponentResponse?> UpdateAsync(int id, ComponentRequest request, string userId);
    Task<bool> DeleteAsync(int id, string userId);
}
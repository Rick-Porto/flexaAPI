using flexa.App.DTOs.Component;

namespace flexa.App.Interfaces;

public interface IComponentService
{
    Task<IEnumerable<ComponentResponse>> GetAllAsync();
    Task<ComponentResponse?> GetByIdAsync(Guid id);
    Task<IEnumerable<ComponentResponse>> GetByScreenIdAsync(Guid screenId);
    Task<ComponentResponse> CreateAsync(ComponentRequest request);
    Task<ComponentResponse?> UpdateAsync(Guid id, ComponentRequest request);
    Task<bool> DeleteAsync(Guid id);
}
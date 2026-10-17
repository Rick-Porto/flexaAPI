using flexa.App.DTOs.AppEntity;

namespace flexa.App.Interfaces
{
    public interface IAppEntityService
    {
        Task<IEnumerable<AppEntityResponse>> GetAllAsync(string userId);
        Task<AppEntityResponse?> GetByIdAsync(Guid id, string userId);
        Task<AppEntityResponse> AddAsync(AppEntityRequest request, string userId);
        Task UpdateAsync(Guid id, AppEntityRequest request, string userId);
        Task DeleteAsync(Guid id, string userId);
        Task<AppEntityResponse?> PublishAsync(Guid id, string userId);
        Task<AppEntityResponse?> UnpublishAsync(Guid id, string userId);
        Task<AppEntityResponse?> GetByPublicLinkAsync(string publicLink);
    }
}
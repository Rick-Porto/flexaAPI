using flexa.App.DTOs.AppEntity;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services
{
    public class AppEntityService : IAppEntityService
    {
        private readonly IAppEntityRepository _repo;

        public AppEntityService(IAppEntityRepository repo)
        {
            _repo = repo;
        }

        private static AppEntityResponse MapToResponse(AppEntity e) => new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            CreatedAt = e.CreatedAt,
            UpdatedAt = e.UpdatedAt,
            PublicLink = e.PublicLink
            // UserId propositalmente não exposto na response
        };

        public async Task<IEnumerable<AppEntityResponse>> GetAllAsync(string userId)
        {
            var entities = await _repo.GetAllAsync(userId);
            return entities.Select(MapToResponse);
        }

        public async Task<AppEntityResponse?> GetByIdAsync(Guid id, string userId)
        {
            var entity = await _repo.GetByIdAsync(id, userId);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<AppEntityResponse> AddAsync(AppEntityRequest appRequest, string userId)
        {
            var app = new Domain.Entities.AppEntity
            {
                Id = Guid.NewGuid(),
                Name = appRequest.Name,
                Description = appRequest.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId  // ✅ vem do JWT, nunca do body do request
            };

            await _repo.AddAsync(app);

            return MapToResponse(app);
        }

        public async Task UpdateAsync(Guid id, AppEntityRequest request, string userId)
        {
            var entity = new AppEntity
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                UserId = userId  // ✅ repositório verifica que este userId é dono da linha
            };

            await _repo.UpdateAsync(entity);
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            await _repo.DeleteAsync(id, userId);
        }

        public async Task<AppEntityResponse?> PublishAsync(Guid id, string userId)
        {
            var entity = await _repo.GetByIdAsync(id, userId);
            if (entity is null) return null;

            // Generate a unique public link if not already set
            if (string.IsNullOrEmpty(entity.PublicLink))
            {
                entity.PublicLink = Guid.NewGuid().ToString("N")[..12]; // 12-char unique link
            }

            entity.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(entity);

            return MapToResponse(entity);
        }

        public async Task<AppEntityResponse?> UnpublishAsync(Guid id, string userId)
        {
            var entity = await _repo.GetByIdAsync(id, userId);
            if (entity is null) return null;

            entity.PublicLink = null;
            entity.UpdatedAt = DateTime.UtcNow;
            await _repo.UpdateAsync(entity);

            return MapToResponse(entity);
        }

        public async Task<AppEntityResponse?> GetByPublicLinkAsync(string publicLink)
        {
            var entity = await _repo.GetByPublicLinkAsync(publicLink);
            return entity is null ? null : MapToResponse(entity);
        }
    }
}
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
            UpdatedAt = e.UpdatedAt
            // UserId propositalmente não exposto na response
        };

        public async Task<IEnumerable<AppEntityResponse>> GetAllAsync(string userId)
        {
            var entities = await _repo.GetAllAsync(userId);
            return entities.Select(MapToResponse);
        }

        // Implement other methods
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

            return new AppEntityResponse
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                CreatedAt = app.CreatedAt,
                UpdatedAt = app.UpdatedAt,
                UserId = userId  // ✅ vem do JWT, nunca do body do request
            };
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
    }
}
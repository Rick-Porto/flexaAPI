using flexa.App.Interfaces;
using flexa.App.DTOs;
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

        public async Task<IEnumerable<AppEntityResponse>> GetAllAsync()
        {
            var apps = await _repo.GetAllAsync();
            return apps.Select(app => new AppEntityResponse
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                CreatedAt = app.CreatedAt,
                UpdatedAt = app.UpdatedAt
            });
        }

        // Implement other methods
        public async Task<AppEntityResponse?> GetByIdAsync(Guid id)
        {
            var app = await _repo.GetByIdAsync(id);
            if (app == null) return null;
            return new AppEntityResponse
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                CreatedAt = app.CreatedAt,
                UpdatedAt = app.UpdatedAt
            };
        }

        public async Task<AppEntityResponse?> AddAsync(AppEntityRequest appRequest)
        {
            var app = new Domain.Entities.AppEntity
            {
                Id = Guid.NewGuid(),
                Name = appRequest.Name,
                Description = appRequest.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(app); 
            
            return new AppEntityResponse
            {
                Id = app.Id,
                Name = app.Name,
                Description = app.Description,
                CreatedAt = app.CreatedAt,
                UpdatedAt = app.UpdatedAt
            };
        }

        public async Task UpdateAsync(Guid id, AppEntityRequest appRequest)
        {
            var app = await _repo.GetByIdAsync(id);
            if (app != null)
            {
                app.Name = appRequest.Name;
                app.Description = appRequest.Description;
                app.UpdatedAt = DateTime.UtcNow;
                await _repo.UpdateAsync(app);
            }
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repo.DeleteAsync(id);
        }
    }
}
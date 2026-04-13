using flexa.App.DTOs.Component;
using flexa.App.DTOs.DataEntry;
using flexa.App.DTOs.Screen;
using flexa.App.Interfaces;
using flexa.Domain.Entities;
using flexa.Domain.Interfaces;

namespace flexa.App.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _repository;

        public ScreenService(IScreenRepository repository) => _repository = repository;

        public async Task<IEnumerable<ScreenResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return entities.Select(e => MapToResponse(e));
        }

        public async Task<ScreenResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<ScreenResponse> CreateAsync(ScreenRequest request)
        {
            var entity = new Screen
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                AppEntityId = request.AppEntityId,
                AppEntity = null!, // Satisfies the required navigation property
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(entity);
            return MapToResponse(created);
        }

        public async Task<ScreenResponse> UpdateAsync(Guid id, ScreenRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) throw new KeyNotFoundException($"Screen with Id {id} not found.");

            existing.Name = request.Name;
            existing.Description = request.Description;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(existing);
            return MapToResponse(updated);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        private static ScreenResponse MapToResponse(Screen entity)
        {
            return new ScreenResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                AppEntityId = entity.AppEntityId,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Components = entity.Components?.Select(c => new ComponentResponse
                {
                    Id = c.Id,
                    ElementType = c.ElementType,
                    Label = c.Label,
                    Model = c.Model,
                    Config = c.Config,
                    Order = c.Order,
                    Required = c.Required,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    DataEntries = c.DataEntries?.Select(de => new DataEntryResponse
                    {
                        Id = de.Id,
                        Config = de.Config,
                        Data = de.Data,
                        CreatedAt = de.CreatedAt,
                        UpdatedAt = de.UpdatedAt,
                        ComponentId = de.ComponentId,
                        ScreenId = de.ScreenId
                    }).ToList()
                }).ToList(),
                DataEntries = entity.DataEntries?.Select(de => new DataEntryResponse
                {
                    Id = de.Id,
                    Config = de.Config,
                    Data = de.Data,
                    CreatedAt = de.CreatedAt,
                    UpdatedAt = de.UpdatedAt,
                    ComponentId = de.ComponentId,
                    ScreenId = de.ScreenId
                }).ToList()
            };
        }
    }
}

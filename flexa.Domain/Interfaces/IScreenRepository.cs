using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces
{
    public interface IScreenRepository
    {
        Task<IEnumerable<Screen>> GetAllAsync();
        Task<Screen?> GetByIdAsync(Guid id);
        Task<Screen> AddAsync(Screen screen);
        Task<Screen> UpdateAsync(Screen screen);
        Task DeleteAsync(Guid id);
    }
}
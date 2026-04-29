using flexa.Domain.Entities;
using System.Collections;

namespace flexa.Domain.Interfaces
{
    public interface IScreenRepository
  {
      Task<IEnumerable> GetAllAsync(string userId);
      Task<Screen?> GetByIdAsync(int id, string userId);
      Task<Screen> AddAsync(Screen entity, string userId);
      Task UpdateAsync(Screen entity, string userId);
      Task DeleteAsync(int id, string userId);
  }

}
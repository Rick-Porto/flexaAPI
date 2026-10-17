using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces
{
    public interface IComponentRepository
    {
        Task<IEnumerable<Component>> GetAllAsync(string userId);
        Task<Component?> GetByIdAsync(int id, string userId);
        Task<IEnumerable<Component>> GetComponentsByScreenIdAsync(int screenId, string userId);
        Task<Component> AddAsync(Component component, string userId);
        Task<Component?> UpdateAsync(Component component, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}
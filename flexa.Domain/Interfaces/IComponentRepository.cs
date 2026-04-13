using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using flexa.Domain.Entities;

namespace flexa.Domain.Interfaces
{
    public interface IComponentRepository
    {
        Task<IEnumerable<Component>> GetAllAsync();
        Task<Component?> GetByIdAsync(Guid id);
        Task<IEnumerable<Component>> GetComponentsByScreenIdAsync(Guid screenId);
        Task<Component> AddAsync(Component component);
        Task<Component> UpdateAsync(Component component);
        Task<bool> DeleteAsync(Guid id);
    }
}
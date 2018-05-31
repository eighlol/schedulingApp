using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllCategories();
        
        Task<Category> Get(Guid id);

        Task<IEnumerable<Category>> GetMainCategories();

        Task<IEnumerable<Category>> GetSubCategories(Guid parentId);

        bool SaveAll();

        void AddCategoryToEvent(Event @event, Guid categoryId);

        Task<IEnumerable<Category>> GetEventCategories(Guid eventId);
    }
}

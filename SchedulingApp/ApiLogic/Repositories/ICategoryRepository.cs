using System;
using System.Collections.Generic;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();

        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetAllEventsDetailed();

        Category GetCategoryById(Guid id);

        IEnumerable<Category> GetMainCategories();

        IEnumerable<Category> GetSubCategories(Guid parentId);

        bool SaveAll();

        void AddCategoryToEvent(Guid eventId, Category newCategory, string username);

        IEnumerable<Category> GetUserCategories(Guid eventId, string username);
    }
}
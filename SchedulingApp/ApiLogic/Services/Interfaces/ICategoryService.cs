using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Requests;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> AddToEvent(Guid eventId);

        Task<IEnumerable<CategoryViewModel>> GetAll();

        Task<CategoryViewModel> GetEventCategories(Guid eventId);

    }
}
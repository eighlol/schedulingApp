using SchedulingApp.ApiLogic.Requests.Dtos;
using System;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface ICategoryService
    {
        Task AddToEvent(Guid eventId, AddCategoryToEventRequest request);

        Task<GetAllCategoriesResponse> GetAll();

        Task<GetEventCategoriesResponse> GetEventCategories(Guid eventId);
    }
}

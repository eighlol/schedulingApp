using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using System;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface IEventService
    {
        Task<GetAllEventResponse> GetAll(string userName);

        Task Create(CreateEventRequest request, string userName);

        Task Delete(Guid eventId);
    }
}

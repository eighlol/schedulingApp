using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using System;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface IEventService
    {
        Task<GetAllEventResponse> GetAll(string userName);

        Task<EventDto> Create(CreateEventRequestDto request, string userName);

        Task Delete(Guid eventId);
    }
}

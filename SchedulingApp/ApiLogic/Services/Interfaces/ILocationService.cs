using System;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface ILocationService
    {
        Task AddToEvent(Guid eventId, AddLocationToEventRequest request);

        Task<GetEventLocationsResponse> GetEventLocations(Guid eventId);

        Task Delete(Guid eventId, Guid locationId);
    }
}

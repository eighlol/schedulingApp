using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using System;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface ILocationService
    {
        Task AddToEvent(Guid eventId, AddLocationToEventRequest request);

        Task<GetEventLocationsResponse> GetEventLocations(Guid eventId);
    }
}


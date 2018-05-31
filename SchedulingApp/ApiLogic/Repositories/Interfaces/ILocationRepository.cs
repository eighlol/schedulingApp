using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        void AddLocation(Event ev, Location newLocation);

        Task<IEnumerable<Location>> GetEventLocations(Guid eventId);

        Task<bool> SaveAll();
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAllEvents();

        Task<IEnumerable<Event>> GetAllEventsDetailed();

        Event GetUserEventByIdDetailed(Guid id, string name);
        
        Event GetEventDetailed(Guid id);

        Task<Event> GetEvent(Guid id);

        void AddEvent(Event newEvent);

        Task<bool> SaveAll();

        Task<IEnumerable<Event>> GetUserAllEventsDetailed(string name);

        void DeleteEvent(Event eventToDelete);
    }
}

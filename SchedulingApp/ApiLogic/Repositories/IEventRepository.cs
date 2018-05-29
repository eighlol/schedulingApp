using System;
using System.Collections.Generic;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<Event> GetAllEvents();

        IEnumerable<Event> GetAllEventsDetailed();

        Event GetUserEventByIdDetailed(Guid id, string name);

        Event GetEventById(Guid id);

        void AddEvent(Event newEvent);

        bool SaveAll();

        IEnumerable<Event> GetUserAllEventsDetailed(string name);

        void DeleteEvent(Event eventToDelete);
    }
}
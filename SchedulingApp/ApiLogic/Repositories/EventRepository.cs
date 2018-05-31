using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp.ApiLogic.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly ILogger<ConferenceRepository> _logger;
        private readonly SchedulingAppDbContext _context;

        public EventRepository(SchedulingAppDbContext context, ILogger<ConferenceRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public Event GetUserEventByIdDetailed(Guid id, string name)
        {
            try
            {
                return _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .Include(i => i.EventMembers).FirstOrDefault(w => w.Id == id && w.UserName == name);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get user event detailed from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public Event GetUserEventById(Guid id, string name)
        {
            try
            {
                return _context.Events.FirstOrDefault(w => w.Id == id && w.UserName == name);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get user event from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public Event GetEventDetailed(Guid id)
        {
            try
            {
                var @event = _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .Include(i => i.EventMembers).FirstOrDefault(w => w.Id == id);

                return @event;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get event from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<Event> GetEvent(Guid id)
        {
            try
            {
                Event @event = await _context.Events.FirstOrDefaultAsync(w => w.Id == id);

                return @event;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get event from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task DeleteEvent(Event eventToDelete)
        {
            try
            {
                List<EventMember> eventMembers = await _context.EventMembers.Where(em => em.EventId == eventToDelete.Id).ToListAsync();
                _context.EventMembers.RemoveRange(eventMembers);

                List<EventCategory> eventCategories = await _context.EventCategories.Where(ec => ec.EventId == eventToDelete.Id).ToListAsync();
                _context.EventCategories.RemoveRange(eventCategories);

                List<Location> locations = await _context.Locations.Where(l => l.Event.Id == eventToDelete.Id).ToListAsync();
                _context.Locations.RemoveRange(locations);

                _context.Events.Remove(eventToDelete);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to delete event from the database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            try
            {
                return await _context.Events.OrderBy(o => o.Name).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get all events. ", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<IEnumerable<Event>> GetAllEventsDetailed()
        {
            try
            {
                return await _context.Events
                    .Include(e => e.Locations)
                    .Include(e => e.EventCategories.Select(ec => ec.Category))
                    .Include(e => e.EventMembers.Select(em => em.Member))
                    .OrderBy(e => e.Name)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get events with locations, categories and members", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<IEnumerable<Event>> GetUserAllEventsDetailed(string name)
        {
            try
            {
                return await _context.Events
                    .Include(e => e.Locations)
                    .Include(e => e.EventCategories)
                        .ThenInclude(ec => ec.Category)
                    .Include(e => e.EventMembers)
                        .ThenInclude(em => em.Member)
                    .OrderBy(e => e.Name)
                    .Where(e => e.UserName == name)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get user events with locations, categories and members", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public void AddEvent(Event newEvent)
        {
            _context.Add(newEvent);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

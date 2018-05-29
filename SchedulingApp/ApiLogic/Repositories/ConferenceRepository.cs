using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp.ApiLogic.Repositories
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly SchedulingAppDbContext _context;
        private readonly ILogger<ConferenceRepository> _logger;

        public ConferenceRepository(SchedulingAppDbContext context, ILogger<ConferenceRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        #region Events

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
                _logger.LogError("Could not get event from database", e);
                return null;
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
                _logger.LogError("Could not get event from database", e);
                return null;
            }
        }

        public Event GetEventById(Guid id)
        {
            try
            {
                return _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .Include(i => i.EventMembers).FirstOrDefault(w => w.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get event from database", e);
                return null;
            }
        }

        public void DeleteEvent(Event eventToDelete)
        {
            try
            {
                _context.Events.Remove(eventToDelete);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete event from database", e);
            }
        }

        public IEnumerable<Event> GetAllEvents()
        {
            try
            {
                return _context.Events.OrderBy(o => o.Name).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get events from database", e);
                return new List<Event>();
            }
        }

        public IEnumerable<Event> GetAllEventsDetailed()
        {
            try
            {
                return _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .OrderBy(o => o.Name)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get events with categories from database", e);
                return new List<Event>();
            }
        }

        public IEnumerable<Event> GetUserAllEventsDetailed(string name)
        {
            try
            {
                return _context.Events
                    .Include(i => i.Locations)
                    .OrderBy(o => o.Name)
                    .Where(w => w.UserName == name)
                    .ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get events with categories and locations from database", e);
                return new List<Event>();
            }
        }

        public void AddEvent(Event newEvent)
        {
            _context.Add(newEvent);
        }

        #endregion

        #region Categories

        public IEnumerable<Category> GetUserCategories(Guid eventId, string username)
        {
            var uEvent = GetUserEventById(eventId, username);
            if (uEvent != null)
            {
                return _context.EventCategories.Where(w => w.EventId == uEvent.Id).Select(s => s.Category).ToList();
            }

            return new List<Category>();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            try
            {
                return _context.Categories.OrderBy(o => o.ParentId);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get categories from database", e);
                return new List<Category>();
            }
        }

        public IEnumerable<Category> GetMainCategories()
        {
            try
            {
                return _context.Categories.Where(w => w.ParentId == Guid.Empty).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get main categories from database", e);
                return new List<Category>();
            }
        }

        public Category GetCategoryById(Guid id)
        {
            try
            {
                return _context.Categories.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get category from database", e);
                return null;
            }
        }

        public IEnumerable<Category> GetSubCategories(Guid parentId)
        {
            try
            {
                return _context.Categories.Where(w => w.Id == parentId).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get subcategories from database", e);
                return new List<Category>();
            }
        }

        public void AddCategoryToEvent(Guid eventId, Category newCategory, string username)
        {
            var ev = GetUserEventById(eventId, username);
            var eventCategory = new EventCategory {Category = newCategory, Event = ev};
            _context.EventCategories.Add(eventCategory);
        }

        #endregion

        #region Locations

        public void AddLocation(Guid eventId, Location location, string username)
        {
            var ev = GetUserEventByIdDetailed(eventId, username);
            ev.Locations.Add(location);
            _context.Locations.Add(location);
        }

        #endregion

        #region Members

        public void AddMemberToEvent(Guid eventId, Member newMember, string username)
        {
            Event ev = GetUserEventById(eventId, username);
            ev.EventMembers.Add(new EventMember() { MemberId = newMember.Id });
            _context.Update(ev);
        }

        public void AddNewMember(Member member)
        {
            _context.Members.Add(member);
        }

        public void DeleteMemberFromEvent(Guid eventId, Member member, string username)
        {
            var ev = GetUserEventById(eventId, username);
            var eventMember = new EventMember {Member = member, Event = ev, EventId = ev.Id, MemberId = member.Id};
            _context.EventMembers.Remove(eventMember);
        }

        public void DeleteAllMembersFromEvent(Guid eventId, string username)
        {
            var ev = GetUserEventByIdDetailed(eventId, username);
            ev.EventMembers = new List<EventMember>();
        }

        public IEnumerable<Member> GetEventMembers(Guid eventId, string username)
        {
            Event uEvent = GetUserEventById(eventId, username);
            if (uEvent != null)
            {
                return _context.EventMembers.Where(w => w.EventId == uEvent.Id).Select(s => s.Member);
            }

            return new List<Member>();
        }

        public Member GetMemberyById(Guid id)
        {
            try
            {
                return _context.Members.FirstOrDefault(w => w.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get member from database", e);
                return null;
            }
        }

        public IEnumerable<Member> GetAllMembers()
        {
            try
            {
                return _context.Members.OrderBy(o => o.Name);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get members from database", e);
                return new List<Member>();
            }
        }

        #endregion

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.Models
{
    public class ConferenceRepository : IConferenceRepository
    {
        private readonly ConferenceDbContext _context;
        private readonly ILogger<ConferenceRepository> _logger;

        public ConferenceRepository(ConferenceDbContext context, ILogger<ConferenceRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        #region Events
        public Event GetUserEventByIdDetailed(int id, string name)
        {
            try
            {
                return _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .Include(i => i.EventMembers)
                    .Where(w => w.Id == id && w.UserName == name).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get event from database", e);
                return null;
            }
        }

        public Event GetUserEventById(int id , string name)
        {
            try
            {
                return _context.Events
                    .Where(w => w.Id == id && w.UserName == name).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get event from database", e);
                return null;
            }
        }

        public Event GetEventById(int id)
        {
            try
            {
                return _context.Events
                    .Include(i => i.EventCategories)
                    .Include(i => i.Locations)
                    .Include(i => i.EventMembers)
                    .Where(w => w.Id == id).FirstOrDefault();
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
                return null;
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
                return null;
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
                return null;
            }
        }
        public void AddEvent(Event newEvent)
        {
            _context.Add(newEvent);
        }
        #endregion

        #region Categories

        public IEnumerable<Category> GetUserCategories(int eventId, string username)
        {
            var uEvent = GetUserEventById(eventId, username);
            if(uEvent != null)
            {
                return _context.EventCategories.Where(w=> w.EventId == uEvent.Id).Select(s => s.Category).ToList();
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
                return null;
            }
        }

        public IEnumerable<Category> GetMainCategories()
        {
            try
            {
                return _context.Categories.Where(w => w.ParentId != 0).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could get main categories from database", e);
                return null;
            }
        }

        public Category GetCategoryById(int id)
        {
            try
            {
                return _context.Categories.Where(w => w.Id == id).FirstOrDefault();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get category from database", e);
                return null;
            }
        }

        public IEnumerable<Category> GetSubCategories(int parentId)
        {
            try
            {
                return _context.Categories.Where(w => w.Id == parentId).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get subcategories from database", e);
                return null;
            }
        }
        public void AddCategoryToEvent(int eventId, Category newCategory, string username)
        {
            var ev = GetUserEventById(eventId, username);
            var eventCategory = new EventCategory { Category = newCategory , Event = ev};
            _context.EventCategories.Add(eventCategory);
        }
        #endregion

        #region Locations
        public void AddLocation(int eventId,Location location, string username)
        {
            var ev = GetUserEventByIdDetailed(eventId, username);
            ev.Locations.Add(location);
            _context.Locations.Add(location);
        }
        #endregion

        #region Members
        public void AddMemberToEvent(int eventId, Member member, string username)
        {
            var ev = GetUserEventById(eventId, username);
            var eventMember = new EventMember { Member = member, Event = ev };
            _context.EventMembers.Add(eventMember);
        }

        public void AddNewMember(Member member)
        {
            _context.Members.Add(member);
        }

        public void DeleteMemberFromEvent(int eventId, Member member, string username)
        {
            var ev = GetUserEventById(eventId, username);
            var eventMember = new EventMember { Member = member, Event = ev, EventId = ev.Id, MemberId = member.Id };
            _context.EventMembers.Remove(eventMember);
        }

        public void DeleteAllMembersFromEvent(int eventId, string username)
        {
            var ev = GetUserEventByIdDetailed(eventId, username);
            ev.EventMembers = new List<EventMember>();
        }

        public IEnumerable<Member> GetEventMembers(int eventId, string username)
        {
            var uEvent = GetUserEventById(eventId, username);
            if (uEvent != null)
            {
                return _context.EventMembers.Where(w => w.EventId == uEvent.Id).Select(s => s.Member);
            }
            return new List<Member>();
        }

        public Member GetMemberyById(int id)
        {
            try
            {
                return _context.Members.Where(w => w.Id == id).FirstOrDefault();
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
                return null;
            }
        }

        #endregion
        public bool SaveAll()
        {
            return _context.SaveChanges() > 0; 
        }

        
    }
}

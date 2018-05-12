using System.Collections.Generic;

namespace Conference.Models
{
    public interface IConferenceRepository
    {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Event> GetAllEvents();
        IEnumerable<Event> GetAllEventsDetailed();
        Category GetCategoryById(int id);
        Event GetUserEventByIdDetailed(int id, string name);
        Event GetEventById(int id);
        IEnumerable<Category> GetMainCategories();
        IEnumerable<Category> GetSubCategories(int parentId);
        void AddEvent(Event newEvent);
        bool SaveAll();
        void AddCategoryToEvent(int eventId, Category newCategory, string username);
        IEnumerable<Category> GetUserCategories(int eventId, string username);
        void AddLocation(int eventId, Location newLocation, string username);
        IEnumerable<Event> GetUserAllEventsDetailed(string name);
        void DeleteEvent(Event eventToDelete);
        void AddMemberToEvent(int eventId, Member newMember, string name);
        void DeleteMemberFromEvent(int eventId, Member newMember, string name);
        void DeleteAllMembersFromEvent(int eventId, string username);
        void AddNewMember(Member member);
        Member GetMemberyById(int id);
        IEnumerable<Member> GetAllMembers();
        IEnumerable<Member> GetEventMembers(int eventId, string username);


    }
}
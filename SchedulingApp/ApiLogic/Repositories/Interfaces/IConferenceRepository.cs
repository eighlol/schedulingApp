using System;
using System.Collections.Generic;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface IConferenceRepository
    {
        IEnumerable<Category> GetAllCategories();
        IEnumerable<Event> GetAllEvents();
        IEnumerable<Event> GetAllEventsDetailed();
        Category GetCategoryById(Guid id);
        Event GetUserEventByIdDetailed(Guid id, string name);
        Event GetEventById(Guid id);
        IEnumerable<Category> GetMainCategories();
        IEnumerable<Category> GetSubCategories(Guid parentId);
        void AddEvent(Event newEvent);
        bool SaveAll();
        void AddCategoryToEvent(Guid eventId, Category newCategory, string username);
        IEnumerable<Category> GetUserCategories(Guid eventId, string username);
        void AddLocation(Guid eventId, Location newLocation, string username);
        IEnumerable<Event> GetUserAllEventsDetailed(string name);
        void DeleteEvent(Event eventToDelete);
        void AddMemberToEvent(Guid eventId, Member newMember, string name);
        void DeleteMemberFromEvent(Guid eventId, Member newMember, string name);
        void DeleteAllMembersFromEvent(Guid eventId, string username);
        void AddNewMember(Member member);
        Member GetMemberyById(Guid id);
        IEnumerable<Member> GetAllMembers();
        IEnumerable<Member> GetEventMembers(Guid eventId, string username);


    }
}
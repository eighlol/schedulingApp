using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        void AddMemberToEvent(Event @event, Guid memberId);

        Task DeleteMemberFromEvent(Guid eventId, Guid memberId);

        void DeleteAllMembersFromEvent(Event @event);

        Task AddNewMember(Member member);

        void Delete(Member member);

        Task<Member> Get(Guid id);

        Task<IEnumerable<Member>> GetAllMembers(string userName);

        Task<IEnumerable<Member>> GetEventMembers(Guid eventId);

        Task<bool> SaveAll();
    }
}

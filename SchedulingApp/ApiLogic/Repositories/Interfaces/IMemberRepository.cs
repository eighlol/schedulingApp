using System;
using System.Collections.Generic;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        void AddMemberToEvent(Guid eventId, Member newMember, string name);

        void DeleteMemberFromEvent(Guid eventId, Member newMember, string name);

        void DeleteAllMembersFromEvent(Guid eventId, string username);

        void AddNewMember(Member member);

        Member GetMemberyById(Guid id);

        IEnumerable<Member> GetAllMembers();

        IEnumerable<Member> GetEventMembers(Guid eventId, string username);
    }
}

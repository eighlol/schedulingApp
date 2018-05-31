using System;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface IMemberService
    {
        Task AddToEvent(Guid eventId, AddMemberToEventRequest request);

        Task Add(AddMemberRequest request, string userName);

        Task DeleteMemberFromEvent(Guid eventId, Guid memberId);

        Task DeleteMember(Guid memberId);

        Task<GetEventMembersResponse> GetEventMembers(Guid eventId);

        Task DeleteMembersFromEvent(Guid eventId);

        Task<GetMemberResponse> GetMembers(string userName);
    }
}

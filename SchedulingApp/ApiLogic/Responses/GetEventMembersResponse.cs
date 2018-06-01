using System;
using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventMembersResponse
    {
        public Guid EventId { get; set; }

        public List<MemberDto> Members { get; set; }
    }
}

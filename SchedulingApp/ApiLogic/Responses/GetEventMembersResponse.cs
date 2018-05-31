using System;
using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventMembersResponse
    {
        public Guid EventId { get; set; }

        public List<Dtos.MemberDto> Members { get; set; }
    }
}

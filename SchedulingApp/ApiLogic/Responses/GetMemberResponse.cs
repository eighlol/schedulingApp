using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetMemberResponse
    {
        public List<Dtos.MemberDto> Members { get; set; }
    }
}
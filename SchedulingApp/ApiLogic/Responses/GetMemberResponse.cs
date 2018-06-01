using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetMemberResponse
    {
        public List<MemberDto> Members { get; set; }
    }
}
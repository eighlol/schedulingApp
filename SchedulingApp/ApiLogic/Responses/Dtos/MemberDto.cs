using System;

namespace SchedulingApp.ApiLogic.Responses.Dtos
{
    public class MemberDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }
    }
}
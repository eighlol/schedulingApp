using System;

namespace SchedulingApp.ApiLogic.Requests.Dtos
{
    public class MemberDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }
    }
}
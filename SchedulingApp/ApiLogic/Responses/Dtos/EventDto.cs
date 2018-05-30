using System;
using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses.Dtos
{
    public class EventDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<LocationDto> Locations { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

        public IEnumerable<MemberDto> Members { get; set; }
    }
}

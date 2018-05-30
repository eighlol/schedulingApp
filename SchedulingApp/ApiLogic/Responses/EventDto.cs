using System;
using System.Collections.Generic;
using SchedulingApp.ApiLogic.Requests;

namespace SchedulingApp.ApiLogic.Responses
{
    public class EventDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<LocationViewModel> Locations { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public IEnumerable<MemberViewModel> Members { get; set; }

    }
}

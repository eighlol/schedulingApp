using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SchedulingApp.ApiLogic.Requests.Dtos;

namespace SchedulingApp.ApiLogic.Requests
{
    public class CreateEventRequest
    {
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        
        public IEnumerable<LocationDto> Locations { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryDto> Categories { get; set; }

        public IEnumerable<MemberDto> Members { get; set; }
    }
}

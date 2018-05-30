using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class CreateEventRequestDto
    {
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        
        public IEnumerable<LocationViewModel> Locations { get; set; }

        public string Description { get; set; }

        public IEnumerable<CategoryViewModel> Categories { get; set; }

        public IEnumerable<MemberViewModel> Members { get; set; }
    }
}

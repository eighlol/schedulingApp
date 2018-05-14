using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public IEnumerable<LocationViewModel> Locations { get; set; }
        public string Description { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<MemberViewModel> Members { get; set; }

    }
}

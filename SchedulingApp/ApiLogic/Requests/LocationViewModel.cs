using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        [Required]
        public DateTime EventStart { get; set; }
        
        [Required]
        public DateTime EventEnd { get; set; }
    }
}

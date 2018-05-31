using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests.Dtos
{
    public class LocationDto
    {
        [Required]
        [StringLength(255, MinimumLength = 5)]
        public string Name { get; set; }
        
        [Required]
        public DateTime EventStart { get; set; }
        
        [Required]
        public DateTime EventEnd { get; set; }
    }
}
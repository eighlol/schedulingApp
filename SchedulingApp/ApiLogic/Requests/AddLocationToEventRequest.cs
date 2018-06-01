using System.ComponentModel.DataAnnotations;
using SchedulingApp.ApiLogic.Requests.Dtos;

namespace SchedulingApp.ApiLogic.Requests
{
    public class AddLocationToEventRequest
    {
        [Required]
        public LocationDto Location { get; set; }     
    }
}

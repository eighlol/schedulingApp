using SchedulingApp.ApiLogic.Requests.Dtos;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class AddLocationToEventRequest
    {
        [Required]
        public LocationDto Location { get; set; }     
    }
}

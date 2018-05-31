using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class AddMemberToEventRequest
    {
        [Required]
        public Guid MemberId { get; set; }        
    }
}

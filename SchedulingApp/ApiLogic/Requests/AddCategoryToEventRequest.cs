using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class AddCategoryToEventRequest
    {
        [Required]
        public Guid CategoryId { get; set; }        
    }
}
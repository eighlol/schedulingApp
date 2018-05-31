using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests.Dtos
{
    public class CategoryDto
    {
        [Required]
        public Guid Id { get; set; }        
    }
}

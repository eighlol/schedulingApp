using System;
using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class CategoryViewModel
    {
        [Required]
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

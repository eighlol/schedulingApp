using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.ApiLogic.Requests
{
    public class AddMemberRequest
    {
        [Required]
        public string Name { get; set; }        

        [Required]
        public string Gender { get; set; }        
    }
}

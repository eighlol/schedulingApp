using System.ComponentModel.DataAnnotations;

namespace SchedulingApp.Domain.Entities
{
    public class Member : AuditableEntity
    {
        [Required]
        public string Name { get; set; }

        public string Gender { get; set; }

    }
}
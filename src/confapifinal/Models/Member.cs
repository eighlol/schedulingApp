using System.ComponentModel.DataAnnotations;

namespace Conference.Models
{
    public class Member
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Sex { get; set; }

    }
}
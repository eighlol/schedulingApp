using System.Collections.Generic;

namespace SchedulingApp.Models
{
    public class Event : AuditableEntity
    {
        public string Name { get; set; }

        public List<Location> Locations { get; set; }

        public string Description { get; set; }

        public string UserName { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; }

        public ICollection<EventMember> EventMembers { get; set; }        
    }
}

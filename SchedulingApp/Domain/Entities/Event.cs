using System.Collections.Generic;

namespace SchedulingApp.Domain.Entities
{
    public class Event : AuditableEntity
    {
        public string Name { get; set; }

        public ICollection<Location> Locations { get; set; } = new List<Location>();

        public string Description { get; set; }

        public string UserName { get; set; }

        public ICollection<EventCategory> EventCategories { get; set; } = new List<EventCategory>();

        public ICollection<EventMember> EventMembers { get; set; } = new List<EventMember>();
    }
}
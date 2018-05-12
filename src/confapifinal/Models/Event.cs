using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Conference.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Location> Locations { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public ICollection<EventCategory> EventCategories { get; set; }
        public ICollection<EventMember> EventMembers { get; set; }        
    }
}

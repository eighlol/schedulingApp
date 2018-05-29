using System;

namespace SchedulingApp.Domain.Entities
{
    public class EventCategory : AuditableEntity
    {
        public Guid EventId { get; set; }

        public Event Event { get; set; }

        public Guid CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
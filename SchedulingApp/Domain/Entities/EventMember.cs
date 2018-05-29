using System;

namespace SchedulingApp.Domain.Entities
{
    public class EventMember
    {
        public Guid EventId { get; set; }

        public Event Event { get; set; }

        public Guid MemberId { get; set; }

        public Member Member { get; set; }
    }
}
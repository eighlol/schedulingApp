using System;

namespace SchedulingApp.Domain.Entities
{
    public class Location : AuditableEntity
    {
        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public DateTime EventStart { get; set; }

        public DateTime EventEnd { get; set; }
    }
}

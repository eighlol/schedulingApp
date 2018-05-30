using System;

namespace SchedulingApp.ApiLogic.Responses.Dtos
{
    public class LocationDto
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
        
        public DateTime EventStart { get; set; }
        
        public DateTime EventEnd { get; set; }
    }
}

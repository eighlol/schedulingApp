using System;
using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventLocationsResponse
    {
        public Guid EventId { get; set; }

        public List<Dtos.LocationDto> Locations { get; set; }
    }
}

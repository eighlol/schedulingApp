using System;
using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventLocationsResponse
    {
        public Guid EventId { get; set; }

        public List<LocationDto> Locations { get; set; }
    }
}

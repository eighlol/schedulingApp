using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetAllEventResponse
    {
        public List<EventDto> Events { get; set; }
    }
}

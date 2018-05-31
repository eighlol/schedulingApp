using System;
using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventCategoriesResponse
    {
        public Guid EventId { get; set; }

        public List<Responses.Dtos.CategoryDto> Categories { get; set; }
    }
}
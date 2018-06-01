using System;
using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetEventCategoriesResponse
    {
        public Guid EventId { get; set; }

        public List<CategoryDto> Categories { get; set; }
    }
}
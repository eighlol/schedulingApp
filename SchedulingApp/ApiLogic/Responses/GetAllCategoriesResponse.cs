using System.Collections.Generic;
using SchedulingApp.ApiLogic.Responses.Dtos;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetAllCategoriesResponse
    {
        public List<CategoryDto> Categories { get; set; }
    }
}

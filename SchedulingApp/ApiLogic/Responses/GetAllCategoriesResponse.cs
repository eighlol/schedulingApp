using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetAllCategoriesResponse
    {
        public List<Responses.Dtos.CategoryDto> Categories { get; set; }
    }
}

using System.Collections.Generic;

namespace SchedulingApp.ApiLogic.Responses
{
    public class GetAllCategoriesResponse
    {
        public List<Dtos.CategoryDto> Categories { get; set; }
    }
}

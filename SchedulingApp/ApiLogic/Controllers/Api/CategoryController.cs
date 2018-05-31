using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingApp.ApiLogic.Requests.Dtos;
using SchedulingApp.ApiLogic.Services.Interfaces;
using System;
using System.Threading.Tasks;
using SchedulingApp.ApiLogic.Requests;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Get event categories.
        /// </summary>
        /// <param name="eventId">Event id</param>
        /// <returns></returns>
        [HttpGet("events/{eventId}/categories")]
        public async Task<IActionResult> GetEventCategories(Guid eventId)
        {
            return Ok(await _categoryService.GetEventCategories(eventId));
        }

        /// <summary>
        /// Get all categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet("categories")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _categoryService.GetAll());
        }
        
        /// <summary>
        /// Add category to the event.
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="request">Category request</param>
        /// <returns></returns>
        [HttpPut("events/{eventId}/categories")]
        public async Task<IActionResult> AddToEvent(Guid eventId, [FromBody]AddCategoryToEventRequest request)
        {
            await _categoryService.AddToEvent(eventId, request);
            return Ok();
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api/events")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("")]
        public IActionResult GetAll()
        {
            return Ok(_eventService.GetAll(User.Identity.Name));
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateNew([FromBody]CreateEventRequestDto request)
        {
            return Ok(await _eventService.Create(request, User.Identity.Name));
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _eventService.Delete(id);
            return NoContent();
        }
    }
}

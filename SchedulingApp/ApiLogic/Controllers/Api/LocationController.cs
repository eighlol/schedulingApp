using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api/events/{eventId}/locations")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        /// <summary>
        /// Returns locations for the event.
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <returns></returns>
        [HttpGet("")]
        public async Task<IActionResult> Get(Guid eventId)
        {
            return Ok(await _locationService.GetEventLocations(eventId));
        }

        /// <summary>
        /// Creates location for the event.
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="request">Location request</param>
        /// <returns></returns>
        [HttpPost("")]
        public async Task<IActionResult> Add(Guid eventId, [FromBody] AddLocationToEventRequest request)
        {
            await _locationService.AddToEvent(eventId, request);
            return Ok();
        }

        /// <summary>
        /// Delete location from the event.
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="locationId">Location Id</param>
        /// <returns></returns>
        [HttpDelete("{locationId}")]
        public async Task<IActionResult> Delete(Guid eventId, Guid locationId)
        {
            await _locationService.Delete(eventId, locationId);
            return NoContent();
        }
    }
}

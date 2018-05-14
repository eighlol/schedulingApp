using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.Models;
using SchedulingApp.Services;
using SchedulingApp.ViewModels;

namespace SchedulingApp.Controllers.Api
{
    [Authorize]
    [Route("api/events/{eventId}/locations")]
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> _logger;
        private readonly IConferenceRepository _repository;
        private readonly CoordService _coordService;

        public LocationController(IConferenceRepository repository, ILogger<LocationController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
        }

        [HttpGet("")]
        public JsonResult Get(Guid eventId)
        {
            try
            {
                var results = _repository.GetUserEventByIdDetailed(eventId, User.Identity.Name);
                if (results != null)
                {
                    return Json(Mapper.Map<IEnumerable<LocationViewModel>>(results.Locations.OrderBy(o => o.EventStart)));
                }
                return Json(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get locations for event {eventId}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding event");
            }
        }

        [HttpPost("")]
        public async Task<JsonResult> Add(Guid eventId, [FromBody] LocationViewModel location)
        {
            try
            {
                if (ModelState.IsValid)
               {
                    var newLocation = Mapper.Map<Location>(location);

                    var coordResult = await _coordService.Lookup(newLocation.Name);
                    if (!coordResult.Success)
                    {
                        Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Json(coordResult.Message);
                    }
                    newLocation.Longitude = coordResult.Longitude;
                    newLocation.Latitude = coordResult.Latitude;

                    _repository.AddLocation(eventId, newLocation, User.Identity.Name);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<LocationViewModel>(newLocation));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save new location", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new location");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new location");
        }
    }
}

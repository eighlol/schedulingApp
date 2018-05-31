using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api/events/{eventId}/locations")]
    public class LocationController : Controller
    {
        private readonly ILogger<LocationController> _logger;
        private readonly IConferenceRepository _repository;
        private readonly ICoordService _coordService;
        private readonly IMapper _mapper;

        public LocationController(IConferenceRepository repository, ICoordService coordService, IMapper mapper, ILogger<LocationController> logger)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public JsonResult Get(Guid eventId)
        {
            try
            {
                var results = _repository.GetUserEventByIdDetailed(eventId, User.Identity.Name);
                if (results != null)
                {
                    return Json(_mapper.Map<IEnumerable<LocationViewModel>>(results.Locations.OrderBy(o => o.EventStart)));
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
                    var newLocation = _mapper.Map<Location>(location);

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
                        return Json(_mapper.Map<LocationViewModel>(newLocation));
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

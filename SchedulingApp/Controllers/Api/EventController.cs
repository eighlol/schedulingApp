using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using SchedulingApp.Models;
using SchedulingApp.ViewModels;

namespace SchedulingApp.Controllers.Api
{
    [Authorize]
    [Route("api/events")]
    public class EventController : Controller
    {
        private readonly IConferenceRepository _repository;
        private readonly ILogger<EventController> _logger;
        private readonly CoordService _coordService;

        public EventController(IConferenceRepository repository, ILogger<EventController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;

        }

        [HttpGet("")]
        public JsonResult GetAll()
        {
            try
            {
                var username = User.Identity.Name;
                var results = Mapper.Map<IEnumerable<EventViewModel>>(_repository.GetUserAllEventsDetailed(username));
                foreach(var ev in results)
                {
                    ev.Categories = Mapper.Map<IEnumerable<CategoryViewModel>>(_repository.GetUserCategories(ev.Id, username));
                    ev.Members = Mapper.Map<IEnumerable<MemberViewModel>>(_repository.GetEventMembers(ev.Id, username));
                }
                
                return Json(results);
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get events",e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding events");
            }            
        }

        [HttpPost("")]
        public async Task<JsonResult> CreateNew([FromBody]EventViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var username = User.Identity.Name;
                    var newEvent = Mapper.Map<Event>(vm);
                    var eventCategories = vm.Categories == null ? new List<Category>() : Mapper.Map<IEnumerable<Category>>(vm.Categories);
                    var eventMembers = vm.Members == null ? new List<Member>() : Mapper.Map<IEnumerable<Member>>(vm.Members);
                    newEvent.UserName = username;
                    foreach (var location in newEvent.Locations)
                    {
                        var coordResult = await _coordService.Lookup(location.Name);
                        if (!coordResult.Success)
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            return Json(coordResult.Message);
                        }
                        location.Longitude = coordResult.Longitude;
                        location.Latitude = coordResult.Latitude;
                    }
                    //for (int i = 0, length = eventCategories.Count() ; i < length; i++)
                    //{
                    //    eventCategories[i] = _repository.GetCategoryById(eventCategories[i].Id);
                    //}

                    //newEvent.EventCategories
                   

                    //Save to DB
                    _logger.LogInformation("Attempting to save event");
                    _repository.AddEvent(newEvent);
                    if (_repository.SaveAll())
                    {
                        _logger.LogInformation("Attempting to save categories to event");
                        foreach (var category in eventCategories)
                        {
                            _repository.AddCategoryToEvent(newEvent.Id, category, username);
                        }
                        foreach (var member in eventMembers)
                        {
                            _repository.AddMemberToEvent(newEvent.Id, member, username);
                        }
                        _repository.SaveAll();
                        
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        var result = Mapper.Map<EventViewModel>(newEvent);
                        result.Categories = Mapper.Map<IEnumerable<CategoryViewModel>>(_repository.GetUserCategories(newEvent.Id, username));
                        result.Members = Mapper.Map<IEnumerable<MemberViewModel>>(_repository.GetEventMembers(newEvent.Id, username));
                        return Json(result);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save new event", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = e.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed", ModelState = ModelState });
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(Guid id)
        {
            try
            {
                var eventToDelte = _repository.GetEventById(id);
                if (eventToDelte != null)
                {
                    _repository.DeleteEvent(eventToDelte);
                    if (_repository.SaveAll())
                    {
                        return Json(true);
                    }
                }
            } catch(Exception e)
            {
                _logger.LogError($"Failed to delete a event {id}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { e.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed"});
        }
    }
}

using AutoMapper;
using Conference.Models;
using Conference.Services;
using Conference.ViewModels;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Conference.Controllers.Api
{
    
    [Authorize]
    [Route("api")]
    public class MemberController : Controller
    {
        private readonly IConferenceRepository _repository;
        private readonly ILogger<EventController> _logger;
        private readonly CoordService _coordService;

        public MemberController(IConferenceRepository repository, ILogger<EventController> logger, CoordService coordService)
        {
            _repository = repository;
            _logger = logger;
            _coordService = coordService;

        }

        [HttpGet("events/{eventId}/members")]
        public JsonResult Get(int eventId)
        {
            try
            {
                var results = _repository.GetEventMembers(eventId, User.Identity.Name);
                if (results == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(null);
                }
                return Json(Mapper.Map<IEnumerable<MemberViewModel>>(results.OrderBy(o => o.Name)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get members for event {eventId}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding event id");
            }
        }
           
                
        [HttpPost("events/{eventId}/members")]
        public JsonResult Add(int eventId, [FromBody] MemberViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newMember = Mapper.Map<Member>(member);
                    
                    _repository.AddMemberToEvent(eventId, newMember, User.Identity.Name);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<MemberViewModel>(_repository.GetMemberyById(newMember.Id)));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save new member", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to save new member");
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new member");
        }
        
        [HttpDelete("events/{eventId}/members/{id}")]
        public JsonResult Delete(int eventId, int id)
        {
            try
            {
                var eventMembers = _repository.GetEventMembers(eventId, User.Identity.Name);
                var member = eventMembers.Where(a => a.Id == id).FirstOrDefault();
                if (member != null)
                {
                    _repository.DeleteMemberFromEvent(eventId, member, User.Identity.Name);
                    if (_repository.SaveAll())
                    {
                        return Json(true);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete a member {id}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = e.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json(new { Message = "Failed" });
        }
        
        [HttpDelete("events/{eventId}/members")]
        public JsonResult DeleteAll(int eventId)
        {
            try
            {
                var eventToDelte = _repository.GetUserEventByIdDetailed(eventId, User.Identity.Name);                
                _repository.DeleteAllMembersFromEvent(eventId, User.Identity.Name);
                    if (_repository.SaveAll())
                    {
                        return Json(true);
                    }                
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete all members {eventId}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = e.Message });
            }
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { Message = "No memebers found" });
        }
        

        [HttpPost("members")]
        public JsonResult AddNewMember([FromBody] MemberViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newMember = Mapper.Map<Member>(member);
                    _repository.AddNewMember(newMember);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(Mapper.Map<MemberViewModel>(newMember));
                    }
                }
            }
            catch (Exception e)
            {

                _logger.LogError($"Failed to add new member", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(new { Message = e.Message });
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new member");
        }

        [HttpGet("members")]
        public JsonResult GetAllMembers()
        {
            try
            {
                var results = _repository.GetAllMembers();
                if (results == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(null);
                }
                return Json(Mapper.Map<IEnumerable<MemberViewModel>>(results));
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get members from database", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occured finding members");
            }
        }
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api")]
    public class MemberController : Controller
    {
        private readonly IConferenceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IConferenceRepository repository, IMapper mapper, ILogger<MemberController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("events/{eventId}/members")]
        public JsonResult Get(Guid eventId)
        {
            try
            {
                IEnumerable<Member> results = _repository.GetEventMembers(eventId, User.Identity.Name);
                if (results == null)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return Json(null);
                }

                return Json(_mapper.Map<IEnumerable<MemberViewModel>>(results.OrderBy(o => o.Name)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get members for event {eventId}", e);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Error occurred finding event id");
            }
        }


        [HttpPost("events/{eventId}/members")]
        public JsonResult Add(Guid eventId, [FromBody] MemberViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newMember = _mapper.Map<Member>(member);

                    _repository.AddMemberToEvent(eventId, newMember, User.Identity.Name);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(_mapper.Map<MemberViewModel>(_repository.GetMemberyById(newMember.Id)));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to save new member", e);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Failed to save new member");
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return Json("Validation failed on new member");
        }

        [HttpDelete("events/{eventId}/members/{id}")]
        public JsonResult Delete(Guid eventId, Guid id)
        {
            try
            {
                var eventMembers = _repository.GetEventMembers(eventId, User.Identity.Name);
                var member = eventMembers.FirstOrDefault(a => a.Id == id);
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
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new {Message = e.Message});
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
            return Json(new {Message = "Failed"});
        }

        [HttpDelete("events/{eventId}/members")]
        public JsonResult DeleteAll(Guid eventId)
        {
            try
            {
                _repository.DeleteAllMembersFromEvent(eventId, User.Identity.Name);
                if (_repository.SaveAll())
                {
                    return Json(true);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to delete all members {eventId}", e);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new {e.Message});
            }

            Response.StatusCode = (int) HttpStatusCode.OK;
            return Json(new {Message = "No memebers found"});
        }


        [HttpPost("members")]
        public JsonResult AddNewMember([FromBody] MemberViewModel member)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newMember = _mapper.Map<Member>(member);
                    _repository.AddNewMember(newMember);

                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(_mapper.Map<MemberViewModel>(newMember));
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add new member", e);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(new { e.Message });
            }

            Response.StatusCode = (int) HttpStatusCode.BadRequest;
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
                    Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return Json(null);
                }

                return Json(_mapper.Map<IEnumerable<MemberViewModel>>(results));
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get members from database", e);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Error occured finding members");
            }
        }
    }
}
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly ILogger<MemberController> _logger;

        public MemberController(IMemberService memberService, ILogger<MemberController> logger)
        {
            _memberService = memberService;
            _logger = logger;
        }

        [HttpGet("events/{eventId}/members")]
        public async Task<IActionResult> Get(Guid eventId)
        {
            return Ok(await _memberService.GetEventMembers(eventId));
        }


        [HttpPut("events/{eventId}/members")]
        public async Task<IActionResult> Add(Guid eventId, [FromBody] AddMemberToEventRequest request)
        {
            await _memberService.AddToEvent(eventId, request);
            return Ok();
        }

        [HttpDelete("events/{eventId}/members/{memberId}")]
        public async Task<IActionResult> Delete(Guid eventId, Guid memberId)
        {
            await _memberService.DeleteMemberFromEvent(eventId, memberId);
            return NoContent();
        }

        [HttpDelete("events/{eventId}/members")]
        public async Task<IActionResult> DeleteAll(Guid eventId)
        {
            await _memberService.DeleteMembersFromEvent(eventId);
            return NoContent();
        }


        [HttpPost("members")]
        public async Task<IActionResult> AddNewMember([FromBody] AddMemberRequest request)
        {
            await _memberService.Add(request, User.Identity.Name);
            return Ok();
        }

        [HttpGet("members")]
        public async Task<IActionResult> GetAllMembers()
        {
            return Ok(await _memberService.GetMembers(User.Identity.Name));
        }
    }
}
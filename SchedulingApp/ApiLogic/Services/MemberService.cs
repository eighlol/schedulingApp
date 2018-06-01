using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Controllers.Api;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Infrastucture.Utils;

namespace SchedulingApp.ApiLogic.Services
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MemberController> _logger;

        public MemberService(IMemberRepository memberRepository, IEventRepository eventRepository, IMapper mapper, ILogger<MemberController> logger)
        {
            _memberRepository = memberRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddToEvent(Guid eventId, AddMemberToEventRequest request)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            var member = await _memberRepository.Get(request.MemberId);

            EnsureMemberExists(member);

            _memberRepository.AddMemberToEvent(@event, request.MemberId);

            await EnsureMemberAddedInDataBase();
        }

        public async Task Add(AddMemberRequest request, string userName)
        {
            var member = _mapper.Map<Member>(request);

            member.UserName = userName;

            await _memberRepository.AddNewMember(member);

            await EnsureMemberSavedInDataBase();
        }

        private async Task EnsureMemberSavedInDataBase()
        {
            if (!await _memberRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to create new member.");
            }
        }

        private static void EnsureEventExists(Event @event)
        {
            if (@event == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, "Event was not found.");
            }
        }

        private void EnsureMemberExists(Member member)
        {
            if (member == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, $"Member was not found.");
            }
        }

        private async Task EnsureMemberAddedInDataBase()
        {
            try
            {
                if (!await _memberRepository.SaveAll())
                {
                    throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to add member to the event.");
                }
            }
            catch (SqlException exception)
            {
                if (exception.IsAnyOfUniqueKeyViolationsError())
                {
                    throw new UseCaseException(HttpStatusCode.Conflict, "Member already exists.");
                }

                throw;
            }
        }

        public async Task DeleteMemberFromEvent(Guid eventId, Guid memberId)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);
            
            var member = await _memberRepository.Get(memberId);

            EnsureMemberExists(member);

            await _memberRepository.DeleteMemberFromEvent(@event.Id, memberId);

            await EnsureMemberDeletedInDataBase();
        }

        private async Task EnsureMemberDeletedInDataBase()
        {
            if (!await _memberRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to delete member from the event.");
            }
        }

        public async Task DeleteMember(Guid memberId)
        {
            var member = await _memberRepository.Get(memberId);

            EnsureMemberExists(member);

            await _memberRepository.Delete(member);

            await EnsureMemberDeletedInDataBase();
        }

        public async Task<GetEventMembersResponse> GetEventMembers(Guid eventId)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            IEnumerable<Member> eventMembers = await _memberRepository.GetEventMembers(eventId);

            var memberDto = _mapper.Map<List<MemberDto>>(eventMembers);

            return new GetEventMembersResponse
            {
                EventId = eventId,
                Members = memberDto
            };
        }

        public async Task DeleteMembersFromEvent(Guid eventId)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            await _memberRepository.DeleteAllMembersFromEvent(eventId);

            await EnsureMembersAreDeletedInDataBase();
        }

        private async Task EnsureMembersAreDeletedInDataBase()
        {
            if (!await _memberRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to delete members from the event.");
            }
        }

        public async Task<GetMemberResponse> GetMembers(string userName)
        {
            IEnumerable<Member> members = await _memberRepository.GetAllMembers(userName);

            var memberDtos = _mapper.Map<List<MemberDto>>(members);

            return new GetMemberResponse
            {
                Members = memberDtos
            };
        }
    }
}
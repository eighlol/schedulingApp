using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp.ApiLogic.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ILogger<MemberRepository> _logger;
        private readonly SchedulingAppDbContext _context;

        public MemberRepository(SchedulingAppDbContext context, ILogger<MemberRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void AddMemberToEvent(Event @event, Guid memberId)
        {
            @event.EventMembers.Add(new EventMember { MemberId = memberId });
            _context.Update(@event);
        }

        public async Task DeleteMemberFromEvent(Guid eventId, Guid memberId)
        {
            EventMember eventMember = await _context.EventMembers.FirstOrDefaultAsync(em => em.EventId == eventId && em.MemberId == memberId);
            _context.EventMembers.Remove(eventMember);
        }

        public async Task DeleteAllMembersFromEvent(Guid eventId)
        {
            List<EventMember> eventMembers = await _context.EventMembers.Where(em => em.EventId == eventId).ToListAsync();
            _context.EventMembers.RemoveRange(eventMembers);
        }

        public async Task AddNewMember(Member member)
        {
            await _context.Members.AddAsync(member);
        }

        public async Task Delete(Member member)
        {
            List<EventMember> eventMembers = await _context.EventMembers.Where(ev => ev.MemberId == member.Id).ToListAsync();
            _context.EventMembers.RemoveRange(eventMembers);
            _context.Members.Remove(member);
        }

        public async Task<Member> Get(Guid id)
        {
            return await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Member>> GetAllMembers(string userName)
        {
            try
            {
                return await _context.Members
                    .Where(m => m.UserName == userName)
                    .OrderBy(m => m.Name)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get all members. ", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<IEnumerable<Member>> GetEventMembers(Guid eventId)
        {
            try
            {
                return await _context.EventMembers
                    .Where(m => m.EventId == eventId)
                    .Select(em => em.Member)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get event members. ", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
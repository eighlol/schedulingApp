using AutoMapper;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>();
        }
    }
}

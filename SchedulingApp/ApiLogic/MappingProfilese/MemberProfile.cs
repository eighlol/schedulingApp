using AutoMapper;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<Member, MemberDto>();

            CreateMap<Requests.Dtos.MemberDto, EventMember>()
                .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.Id));

            CreateMap<AddMemberToEventRequest, Member>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.MemberId));

            CreateMap<AddMemberRequest, Member>();
        }
    }
}

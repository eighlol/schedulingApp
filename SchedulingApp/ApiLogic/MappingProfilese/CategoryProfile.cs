using AutoMapper;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Event, GetAllEventResponse>().ReverseMap();
            CreateMap<Category, CategoryViewModel>().ReverseMap();
            CreateMap<Location, LocationViewModel>().ReverseMap();
            CreateMap<Member, MemberViewModel>().ReverseMap();
        }
    }
}
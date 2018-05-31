using AutoMapper;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.Domain.Entities;
using System.Linq;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.CreationDate))
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.EventCategories.Select(ec => ec.Category)))
                .ForMember(dest => dest.Members, opt => opt.MapFrom(src => src.EventMembers.Select(em => em.Member)));

            CreateMap<CreateEventRequest, Event>()
                .ForMember(dest => dest.Locations, opt => opt.MapFrom(src => src.Locations))
                .ForMember(dest => dest.EventCategories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.EventMembers, opt => opt.MapFrom(src => src.Members));
        }
    }
}

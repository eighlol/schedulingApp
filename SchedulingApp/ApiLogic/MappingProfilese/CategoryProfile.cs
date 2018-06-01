using AutoMapper;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


            CreateMap<Requests.Dtos.CategoryDto, EventCategory>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}

using AutoMapper;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.MappingProfilese
{
    public class LocationProfile : Profile
    {
        public LocationProfile()
        {
            CreateMap<Location, LocationDto>();

            CreateMap<Requests.Dtos.LocationDto, Location>();
        }
    }
}
//TODO: search dlja kategorij 
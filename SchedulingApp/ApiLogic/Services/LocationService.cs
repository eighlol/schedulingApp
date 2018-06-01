using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;

namespace SchedulingApp.ApiLogic.Services
{
    public class LocationService : ILocationService
    {
        private readonly ICoordService _coordService;
        private readonly ILocationRepository _locationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ICoordService coordService, ILocationRepository locationRepository,
            IEventRepository eventRepository, IMapper mapper, ILogger<LocationService> logger)
        {
            _coordService = coordService;
            _locationRepository = locationRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddToEvent(Guid eventId, AddLocationToEventRequest request)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            var newLocation = _mapper.Map<Location>(request.Location);

            await ValidateLocation(newLocation);

            _locationRepository.AddLocation(@event, newLocation);

            await EnsureLocationIsSavedInDataBase();
        }

        private static void EnsureEventExists(Event @event)
        {
            if (@event == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, "Event was not found.");
            }
        }

        private async Task ValidateLocation(Location newLocation)
        {
            CoordServiceResult coordResult = await _coordService.Lookup(newLocation.Name);

            if (!coordResult.Success)
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, coordResult.Message);
            }

            newLocation.Longitude = coordResult.Longitude;
            newLocation.Latitude = coordResult.Latitude;
        }

        private async Task EnsureLocationIsSavedInDataBase()
        {
            if (!await _locationRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to add location to the event.");
            }
        }

        public async Task<GetEventLocationsResponse> GetEventLocations(Guid eventId)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            IEnumerable<Location> locations = await _locationRepository.GetEventLocations(eventId);

            var locationsDto = _mapper.Map<List<LocationDto>>(locations);

            return new GetEventLocationsResponse
            {
                EventId = eventId,
                Locations = locationsDto
            };
        }

        public async Task Delete(Guid eventId, Guid locationId)
        {
            var @event = await _eventRepository.Get(eventId);

            EnsureEventExists(@event);

            var location = await _locationRepository.GetLocation(locationId);

            EnsureLocationExists(location);

            _locationRepository.Delete(location);

            await EnsureLocationIsDeletedInDataBase();
        }

        private void EnsureLocationExists(Location location)
        {
            if (location == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, "Location was not found.");
            }
        }

        private async Task EnsureLocationIsDeletedInDataBase()
        {
            if (!await _locationRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to remove location from the event.");
            }
        }
    }
}
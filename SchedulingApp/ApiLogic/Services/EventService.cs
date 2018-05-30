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
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ICoordService _coordService;
        private readonly IMapper _mapper;
        private readonly ILogger<EventService> _logger;

        public EventService(
            IEventRepository eventRepository,
            ICoordService coordService,
            IMapper mapper,
            ILogger<EventService> logger
        )
        {
            _eventRepository = eventRepository;
            _coordService = coordService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllEventResponse> GetAll(string userName)
        {
            _logger.LogInformation($"Getting all events for user with username: {userName}");

            IEnumerable<Event> userAllEventsDetailed = await _eventRepository.GetUserAllEventsDetailed(userName);

            var events = _mapper.Map<List<EventDto>>(userAllEventsDetailed);

            return new GetAllEventResponse
            {
                Events = events
            };
        }

        public async Task<EventDto> Create(CreateEventRequestDto request, string userName)
        {
            var newEvent = _mapper.Map<Event>(request);

            newEvent.UserName = userName;

            await ValidateLocations(newEvent.Locations);

            _eventRepository.AddEvent(newEvent);

            if (await _eventRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to create a new event.");
            }

            var result = _mapper.Map<EventDto>(newEvent);

            return result;
        }

        private async Task ValidateLocations(IEnumerable<Location> locations)
        {
            foreach (var location in locations)
            {
                var coordResult = await _coordService.Lookup(location.Name);

                if (!coordResult.Success)
                {
                    throw new UseCaseException(HttpStatusCode.BadRequest, coordResult.Message);
                }

                location.Longitude = coordResult.Longitude;
                location.Latitude = coordResult.Latitude;
            }
        }

        public async Task Delete(Guid eventId)
        {
            var eventToDelte = _eventRepository.GetEventDetailedById(eventId);

            EnsureEventExists(eventId, eventToDelte);

            _eventRepository.DeleteEvent(eventToDelte);

            await EnsureEventDeletedInDatabase();
        }

        private async Task EnsureEventDeletedInDatabase()
        {
            if (!await _eventRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to delete event.");
            }
        }

        private void EnsureEventExists(Guid eventId, Event eventToDelte)
        {
            if (eventToDelte == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, $"Event was not found with id {eventId}.");
            }
        }
    }
}

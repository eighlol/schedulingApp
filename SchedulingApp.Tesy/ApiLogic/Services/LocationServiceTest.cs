using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchedulingApp.ApiLogic.MappingProfilese;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.ApiLogic.Services;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Tesy.TestUtils;

namespace SchedulingApp.Tesy.ApiLogic.Services
{
    public class LocationServiceTest
    {
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<ICoordService> _coordServiceMock;
        private Mock<ILocationRepository> _locationRepositoryMock;
        private Mock<ILogger<LocationService>> _loggerMock;

        private ILocationService _locationService;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<EventProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<LocationProfile>();
                cfg.AddProfile<MemberProfile>();
            });

            _coordServiceMock = new Mock<ICoordService>();
            _eventRepositoryMock = new Mock<IEventRepository>();
            _locationRepositoryMock = new Mock<ILocationRepository>();
            _loggerMock = new Mock<ILogger<LocationService>>();
            _locationService = new LocationService(_coordServiceMock.Object, _locationRepositoryMock.Object, _eventRepositoryMock.Object, Mapper.Instance, _loggerMock.Object);
        }

        public void TearDown()
        {
            _eventRepositoryMock.Reset();
            _locationRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task GetEventLocations_ShouldSuccess()
        {
            var eventId = new Guid("EECB7DF6-A46A-4AE1-970F-2041F62525E8");
            const string locationName = "Skolas iela 1, Riga, Latvia";
            const double latitude = 22;
            const double longitude = 33;
            var eventStart = new DateTime(1999, 12, 1);
            var eventEnd = new DateTime(1999, 12, 2);

            var locations = new List<Location>
            {
                new Location
                {
                    Name = locationName,
                    Longitude = longitude,
                    Latitude = latitude,
                    EventStart = eventStart,
                    EventEnd = eventEnd
                }
            };

            var @event = new Event
            {
                Id = eventId
            };

            _eventRepositoryMock.Setup(er => er.Get(eventId)).ReturnsAsync(@event);

            _locationRepositoryMock.Setup(lr => lr.GetEventLocations(eventId))
                .ReturnsAsync(locations);

            GetEventLocationsResponse actual = await _locationService.GetEventLocations(eventId);

            var expected = new GetEventLocationsResponse
            {
                EventId = eventId,
                Locations = new List<LocationDto>
                {
                    new LocationDto
                    {
                        EventStart = eventStart,
                        EventEnd = eventEnd,
                        Longitude = longitude,
                        Latitude = latitude,
                        Name = locationName
                    }
                }

            };

            ContentAssert.AreEqual(expected, actual);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SchedulingApp.ApiLogic.MappingProfilese;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Responses.Dtos;
using SchedulingApp.ApiLogic.Services;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Tesy.TestUtils;
using CategoryDto = SchedulingApp.ApiLogic.Requests.Dtos.CategoryDto;
using LocationDto = SchedulingApp.ApiLogic.Requests.Dtos.LocationDto;
using MemberDto = SchedulingApp.ApiLogic.Requests.Dtos.MemberDto;

namespace SchedulingApp.Tesy.ApiLogic.Services
{
    public class EventServiceTest
    {
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<ICoordService> _coordServiceMock;
        private Mock<ILogger<EventService>> _loggerMock;

        private IEventService _eventService;

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

            _eventRepositoryMock = new Mock<IEventRepository>();
            _coordServiceMock = new Mock<ICoordService>();
            _loggerMock = new Mock<ILogger<EventService>>();
            _eventService = new EventService(_eventRepositoryMock.Object, _coordServiceMock.Object,
                Mapper.Instance, _loggerMock.Object);
        }

        public void TearDown()
        {
            _eventRepositoryMock.Reset();
            _coordServiceMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task CreateNew_ShouldSuccess()
        {
            //ARRANGE
            const string userName = "userName";
            const string eventDescription = "event description";
            const string eventName = "event name";
            const string locationName = "Skolas iela 1, Riga, Latvia";
            const double latitude = 22;
            const double longitude = 33;
            DateTime? eventStart = new DateTime(1999, 12, 1);
            DateTime? eventEnd = new DateTime(1999, 12, 2);
            var memberId = new Guid("41FEB6C1-DA81-4E9D-8ABA-63665DF0B9CC");
            var categoryId = new Guid("7BC0C011-7FA3-4AA4-B260-257465831D65");

            var coordServiceResult = new CoordServiceResult
            {
                Latitude = latitude,
                Longitude = longitude,
                Message = "location message",
                Success = true
            };

            var createEventRequest = new CreateEventRequest
            {
                Categories = new List<CategoryDto> {new CategoryDto {CategoryId = categoryId}},
                Locations = new List<LocationDto>
                {
                    new LocationDto {EventStart = eventStart, EventEnd = eventEnd, Name = locationName}
                },
                Members = new List<MemberDto> {new MemberDto {Id = memberId}},
                Name = eventName,
                Description = eventDescription
            };

            Event @event = null;

            _eventRepositoryMock.Setup(er => er.AddEvent(It.IsNotNull<Event>()))
                .Callback<Event>(e => @event = e);

            _eventRepositoryMock.Setup(er => er.SaveAll()).ReturnsAsync(true);

            _coordServiceMock.Setup(cs => cs.Lookup(locationName))
                .ReturnsAsync(coordServiceResult);

            //ACT
            await _eventService.Create(createEventRequest, userName);

            //ASSERT
            var expected = new Event
            {
                UserName = userName,
                EventMembers = new List<EventMember>
                {
                    new EventMember
                    {
                        MemberId = memberId
                    }
                },
                EventCategories = new List<EventCategory>
                {
                    new EventCategory
                    {
                        CategoryId = categoryId
                    }
                },
                Locations = new List<Location>
                {
                    new Location
                    {
                        EventEnd = eventEnd.Value,
                        EventStart = eventStart.Value,
                        Latitude = latitude,
                        Longitude = longitude,
                        Name = locationName
                    }
                },
                Description = eventDescription,
                Name = eventName
            };

            ContentAssert.AreEqual(expected, @event);
        }

        [Test]
        public void CreateNew_WhenLocationIsNotValid_ShouldThrowUseCaseException()
        {
            //ARRANGE
            const string userName = "userName";
            const string eventDescription = "event description";
            const string eventName = "event name";
            const string locationName = "Skolas iela, Riga, Latvia";
            const double latitude = 22;
            const double longitude = 33;
            DateTime? eventStart = new DateTime(1999, 12, 1);
            DateTime? eventEnd = new DateTime(1999, 12, 2);
            var memberId = new Guid("41FEB6C1-DA81-4E9D-8ABA-63665DF0B9CC");
            var categoryId = new Guid("7BC0C011-7FA3-4AA4-B260-257465831D65");

            var coordServiceResult = new CoordServiceResult
            {
                Latitude = latitude,
                Longitude = longitude,
                Message = "location message",
                Success = false
            };

            var createEventRequest = new CreateEventRequest
            {
                Categories = new List<CategoryDto> {new CategoryDto {CategoryId = categoryId}},
                Locations = new List<LocationDto>
                {
                    new LocationDto {EventStart = eventStart, EventEnd = eventEnd, Name = locationName}
                },
                Members = new List<MemberDto> {new MemberDto {Id = memberId}},
                Name = eventName,
                Description = eventDescription
            };

            _coordServiceMock.Setup(cs => cs.Lookup(locationName))
                .ReturnsAsync(coordServiceResult);

            //ACT & ASSERT
            Assert.ThrowsAsync<UseCaseException>(() => _eventService.Create(createEventRequest, userName));
        }

        [Test]
        public async Task GetAll_ShouldSuccess()
        {
            //ARRANGE
            const string userName = "userName";
            const string eventDescription = "event description";
            const string eventName = "event name";
            const string locationName = "Skolas iela 1, Riga, Latvia";
            const double latitude = 22;
            const double longitude = 33;
            const string catDescr = "cat descr";
            const string catName = "cat name";
            const string memberName = "John";
            const string gender = "male";
            var eventStart = new DateTime(1999, 12, 1);
            var eventEnd = new DateTime(1999, 12, 2);
            var memberId = new Guid("41FEB6C1-DA81-4E9D-8ABA-63665DF0B9CC");
            var categoryId = new Guid("7BC0C011-7FA3-4AA4-B260-257465831D65");
            var eventId = new Guid("AE5CD780-EC30-41BA-BF1A-501CAF172E53");

            var events = new List<Event>
            {
                new Event
                {
                    Description = eventDescription,
                    Name = eventName,
                    UserName = userName,
                    Locations = new List<Location>
                    {
                        new Location
                        {
                            Name = locationName,
                            Longitude = longitude,
                            Latitude = latitude,
                            EventStart = eventStart,
                            EventEnd = eventEnd
                        }
                    },
                    EventMembers = new List<EventMember>
                    {
                        new EventMember
                        {
                            Member = new Member
                            {
                                Gender = gender,
                                Name = memberName,
                                Id = memberId
                            }
                        }
                    },
                    EventCategories = new List<EventCategory>
                    {
                        new EventCategory
                        {
                            Category = new Category
                            {
                                Id = categoryId,
                                Description = catDescr,
                                Name = catName
                            }
                        }
                    },
                    Id = eventId
                }
            };

            _eventRepositoryMock.Setup(er => er.GetUserAllEventsDetailed(userName))
                .ReturnsAsync(events);

            //ACT
            GetAllEventResponse actual = await _eventService.GetAll(userName);

            //ASSERT
            var expected = new GetAllEventResponse
            {
                Events = new List<EventDto>
                {
                    new EventDto
                    {
                        Id = eventId,
                        Categories = new List<SchedulingApp.ApiLogic.Responses.Dtos.CategoryDto>
                        {
                            new SchedulingApp.ApiLogic.Responses.Dtos.CategoryDto
                            {
                                Id = categoryId,
                                Description = catDescr,
                                Name = catName
                            }
                        },
                        Description = eventDescription,
                        Name = eventName,
                        Locations = new List<SchedulingApp.ApiLogic.Responses.Dtos.LocationDto>
                        {
                            new SchedulingApp.ApiLogic.Responses.Dtos.LocationDto
                            {
                                EventStart = eventStart,
                                EventEnd = eventEnd,
                                Longitude = longitude,
                                Latitude = latitude,
                                Name = locationName
                            }
                        },
                        Members = new List<SchedulingApp.ApiLogic.Responses.Dtos.MemberDto>
                        {
                            new SchedulingApp.ApiLogic.Responses.Dtos.MemberDto
                            {
                                Gender = gender,
                                Name = memberName,
                                Id = memberId
                            }
                        }
                    }
                }
            };

            ContentAssert.AreEqual(expected, actual);
        }

        [Test]
        public async Task Delete_ShouldCallRepository()
        {
            var eventId = new Guid("48CBBFCD-7368-4E38-A947-D23CAFD4DE83");
            var @event = new Event
            {
                Id = eventId
            };

            _eventRepositoryMock.Setup(er => er.Get(eventId))
                .ReturnsAsync(@event);

            _eventRepositoryMock.Setup(er => er.DeleteEvent(@event)).Returns(Task.CompletedTask);
            _eventRepositoryMock.Setup(er => er.SaveAll()).ReturnsAsync(true);

            await _eventService.Delete(eventId);

            _eventRepositoryMock.Verify(er => er.DeleteEvent(@event), Times.Once);
            _eventRepositoryMock.Verify(er => er.SaveAll(), Times.Once);
        }
    }
}
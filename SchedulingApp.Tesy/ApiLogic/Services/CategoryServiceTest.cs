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
    public class CategoryServiceTest
    {
        private Mock<IEventRepository> _eventRepositoryMock;
        private Mock<ICategoryRepository> _categoryRepositoryMock;
        private Mock<ILogger<CategoryService>> _loggerMock;

        private ICategoryService _categoryService;

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
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _loggerMock = new Mock<ILogger<CategoryService>>();
            _categoryService = new CategoryService(_categoryRepositoryMock.Object, _eventRepositoryMock.Object,
                Mapper.Instance, _loggerMock.Object);
        }

        public void TearDown()
        {
            _eventRepositoryMock.Reset();
            _categoryRepositoryMock.Reset();
            _loggerMock.Reset();
        }

        [Test]
        public async Task CetEventCategories_ShouldSuccess()
        {
            //ARRANGE
            const string catDesc = "cat desc";
            const string catName = "cat name";
            var categoryId = new Guid("7BC0C011-7FA3-4AA4-B260-257465831D65");
            var eventId = new Guid("DDB2B2AB-D122-40FA-B117-7DB88F48FA23");

            var @event = new Event
            {
                Id = eventId
            };

            var categories = new List<Category>
            {
                new Category
                {
                    Description = catDesc,
                    Name = catName,
                    Id = categoryId
                }
            };

            _eventRepositoryMock.Setup(er => er.Get(eventId))
                .ReturnsAsync(@event);


            _categoryRepositoryMock.Setup(cr => cr.GetEventCategories(eventId))
                .ReturnsAsync(categories);

            //ACT
            GetEventCategoriesResponse actual = await _categoryService.GetEventCategories(eventId);

            //ASSERT
            var expected = new GetEventCategoriesResponse
            {
                EventId = eventId,
                Categories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        Description = catDesc,
                        Name = catName,
                        Id = categoryId
                    }
                }
            };

            ContentAssert.AreEqual(expected, actual);
        }

        [Test]
        public async Task GetAll_ShouldSuccess()
        {
            //ARRANGE
            const string catDesc = "cat desc";
            const string catDesc2 = "cat desc2";
            const string catName = "cat name";
            const string catName2 = "cat name2";
            var categoryId = new Guid("7BC0C011-7FA3-4AA4-B260-257465831D65");
            var categoryId2 = new Guid("FB7871B2-B08F-487F-9739-0172C77A4BDD");
           

            var categories = new List<Category>
            {
                new Category
                {
                    Description = catDesc,
                    Name = catName,
                    Id = categoryId
                },
                new Category
                {
                    Description = catDesc,
                    Name = catName,
                    Id = categoryId
                }
            };
            

            _categoryRepositoryMock.Setup(cr => cr.GetAllCategories())
                .ReturnsAsync(categories);

            //ACT
            GetAllCategoriesResponse actual = await _categoryService.GetAll();

            //ASSERT
            var expected = new GetAllCategoriesResponse
            {
                Categories = new List<CategoryDto>
                {
                    new CategoryDto
                    {
                        Description = catDesc,
                        Name = catName,
                        Id = categoryId
                    },
                    new CategoryDto
                    {
                        Description = catDesc2,
                        Name = catName2,
                        Id = categoryId2
                    }
                }
            };

            ContentAssert.AreCollectionsEquivalent(expected.Categories, actual.Categories);
        }
    }
}

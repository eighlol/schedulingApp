﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using SchedulingApp.ApiLogic.Responses;
using SchedulingApp.ApiLogic.Services.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, IEventRepository eventRepository, IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _eventRepository = eventRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddToEvent(Guid eventId, AddCategoryToEventRequest request)
        {
            Event @event = await _eventRepository.GetEvent(eventId);

            EnsureEventExists(@event);

            await EnsureCategoryExists(request);

            _categoryRepository.AddCategoryToEvent(@event, request.CategoryId);

            EnsureCategoryAddedInDatabase();
        }

        private void EnsureEventExists(Event @event)
        {
            if (@event == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, $"Event was not found.");
            }
        }

        private async Task EnsureCategoryExists(AddCategoryToEventRequest request)
        {
            Category category = await _categoryRepository.Get(request.CategoryId);
            if (category == null)
            {
                throw new UseCaseException(HttpStatusCode.NotFound, $"Category was not found with id {request.CategoryId}.");
            }
        }

        private void EnsureCategoryAddedInDatabase()
        {
            if (!_categoryRepository.SaveAll())
            {
                throw new UseCaseException(HttpStatusCode.BadRequest, "Failed to add category.");
            }
        }

        public async Task<GetAllCategoriesResponse> GetAll()
        {
            IEnumerable<Category> categories = await _categoryRepository.GetAllCategories();

            var categoriesDto = _mapper.Map<List<Responses.Dtos.CategoryDto>>(categories);

            return new GetAllCategoriesResponse
            {
                Categories = categoriesDto
            };
        }

        public async Task<GetEventCategoriesResponse> GetEventCategories(Guid eventId)
        {
            _logger.LogInformation($"Getting event categories for event {eventId}.");

            Event @event = await _eventRepository.GetEvent(eventId);

            EnsureEventExists(@event);

            var categories = await _categoryRepository.GetEventCategories(eventId);

            var categoriesDto = _mapper.Map<List<Responses.Dtos.CategoryDto>>(categories);

            return new GetEventCategoriesResponse
            {
                EventId = eventId,
                Categories = categoriesDto
            };
        }
    }
}
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.ApiLogic.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SchedulingApp.ApiLogic.Controllers.Api
{
    [Authorize]
    [Route("api")]
    public class CategoryController : Controller
    {
        private readonly IConferenceRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(IConferenceRepository repository, IMapper mapper, ILogger<CategoryController> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("events/{eventId}/categories")]
        public JsonResult GetEventCategories(Guid eventId)
        {
            try
            {
                var results = _repository.GetUserCategories(eventId, User.Identity.Name);
                if (results == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(null);
                }
                return Json(_mapper.Map<IEnumerable<CategoryViewModel>>(results.OrderBy(o => o.ParentId)));
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to get catorgies for event {eventId}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occurred finding event id");
            }
        }

        [HttpGet("categories")]
        public JsonResult GetAll()
        {
            try
            {
                var results = _repository.GetAllCategories();
                if (results == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotFound;
                    return Json(null);
                }
                return Json(_mapper.Map<IEnumerable<CategoryViewModel>>(results));
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get categories from database", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Error occured finding categories");
            }
        }
        
        [HttpPost("events/{eventId}/categories")]
        public JsonResult AddToEvent(Guid eventId, [FromBody]CategoryViewModel category)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newCategory = _repository.GetCategoryById(category.Id);
                    if (newCategory == null)
                    {
                        Response.StatusCode = (int)HttpStatusCode.NotFound;
                        return Json(null);
                    }
                    _repository.AddCategoryToEvent(eventId, newCategory, User.Identity.Name);
                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(_mapper.Map<CategoryViewModel>(newCategory));
                    }                    
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to add category for event {eventId}", e);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Failed to add category");
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return Json("Validation failed on new category");
        }
    }
}


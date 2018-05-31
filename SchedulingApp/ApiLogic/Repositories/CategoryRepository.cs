using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SchedulingApp.ApiLogic.Repositories.Interfaces;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Middleware.Exception;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp.ApiLogic.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly SchedulingAppDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(SchedulingAppDbContext context, ILogger<CategoryRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        
        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            try
            {
                return await _context.Categories.OrderBy(o => o.ParentId).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all categories from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data.");
            }
        }

        public async Task<IEnumerable<Category>> GetMainCategories()
        {
            try
            {
                return await _context.Categories.Where(w => w.ParentId == Guid.Empty).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get main categories from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data.");
            }
        }

        public async Task<Category> Get(Guid id)
        {
            try
            {
                return await _context.Categories.FirstOrDefaultAsync(w => w.Id == id);
            }
            catch (Exception e)
            {
                _logger.LogError("Could not category from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data.");
            }
        }

        public async Task<IEnumerable<Category>> GetSubCategories(Guid parentId)
        {
            try
            {
                return await _context.Categories.Where(w => w.Id == parentId).ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get sub categories from database", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data.");
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public void AddCategoryToEvent(Event @event, Guid categoryId)
        {
            @event.EventCategories.Add(new EventCategory{CategoryId = categoryId});
            _context.Update(@event);
        }

        public async Task<IEnumerable<Category>> GetEventCategories(Guid eventId)
        {
            try
            {
                return await _context.EventCategories
                    .Where(ec => ec.EventId == eventId)
                    .Select(ec => ec.Category)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get event categories", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data.");
            }
        }
    }
}
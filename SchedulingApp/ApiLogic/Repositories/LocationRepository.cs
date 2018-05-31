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
    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger<LocationRepository> _logger;
        private readonly SchedulingAppDbContext _context;

        public LocationRepository(SchedulingAppDbContext context, ILogger<LocationRepository> logger)
        {
            _logger = logger;
            _context = context;
        }
        
        public void AddLocation(Event ev, Location newLocation)
        {
            ev.Locations.Add(newLocation);
            _context.Update(ev);
        }

        public async Task<IEnumerable<Location>> GetEventLocations(Guid eventId)
        {
            try
            {
                List<Location> locations = await _context.Locations.Where(l => l.Event.Id == eventId).ToListAsync();
                return locations;
            }
            catch (Exception e)
            {
                _logger.LogError("Failed to get event locations. ", e);
                throw new UseCaseException(HttpStatusCode.InternalServerError, "Failed to access data");
            }
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}

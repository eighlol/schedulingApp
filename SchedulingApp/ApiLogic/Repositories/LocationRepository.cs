using Microsoft.Extensions.Logging;
using SchedulingApp.Domain.Entities;
using SchedulingApp.Infrastucture.Sql;

namespace SchedulingApp.ApiLogic.Repositories
{
    public class LocationRepository : ILocationRepository
    {
        private readonly ILogger<ConferenceRepository> _logger;
        private readonly SchedulingAppDbContext _context;

        public LocationRepository(SchedulingAppDbContext context, ILogger<ConferenceRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void AddLocation(Event ev, Location newLocation, string username)
        {
            _logger.LogInformation("Adding location to the event.");
            //var ev = GetUserEventByIdDetailed(eventId, username);
            ev.Locations.Add(newLocation);
            _context.Locations.Add(newLocation);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

    }
}
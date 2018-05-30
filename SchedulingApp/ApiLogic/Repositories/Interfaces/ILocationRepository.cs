using SchedulingApp.Domain.Entities;

namespace SchedulingApp.ApiLogic.Repositories.Interfaces
{
    public interface ILocationRepository
    {
        bool SaveAll();

        void AddLocation(Event ev, Location newLocation, string username);
    }
}

using System.Threading.Tasks;

namespace SchedulingApp.ApiLogic.Services.Interfaces
{
    public interface ICoordService
    {
        Task<CoordServiceResult> Lookup(string location);
    }
}
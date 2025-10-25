using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UrbanIndicatorsSystem.Services
{
    public interface ITrafficService
    {
        Task<List<TrafficData>> GetTrafficData();
        Task SimulateTraffic();
    }
}
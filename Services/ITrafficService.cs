using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public interface ITrafficService
    {
        List<TrafficData> GetTrafficData();
        void SimulateTraffic();
    }
}

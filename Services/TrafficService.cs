using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public class TrafficService : ITrafficService
    {
        public IEnumerable<TrafficData> GetTrafficForArea(string areaName)
        {
            return new List<TrafficData>
            {
                new TrafficData { RoadName = "Вулиця Хрещатик", TrafficLevel = "Середній" },
                new TrafficData { RoadName = "Проспект Перемоги", TrafficLevel = "Високий" }
            };
        }
    }
}

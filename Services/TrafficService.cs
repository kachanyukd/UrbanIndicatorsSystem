using System;
using System.Collections.Generic;
using UrbanIndicatorsSystem.Models;

namespace UrbanIndicatorsSystem.Services
{
    public class TrafficService : ITrafficService
    {
        private readonly List<TrafficData> _trafficData;
        private readonly System.Timers.Timer _timer; 

        private readonly string[] _trafficLevels = new[]
        {
            "Low",
            "Comfortable",
            "Moderate",
            "Medium",
            "High"
        };

        public TrafficService()
        {
            _trafficData = new List<TrafficData>
            {
                new TrafficData { RoadName = "Shevchenkivskyi", TrafficLevel = "Low" },
                new TrafficData { RoadName = "Pechersk", TrafficLevel = "Comfortable" },
                new TrafficData { RoadName = "Holosiivskyi", TrafficLevel = "Moderate" },
                new TrafficData { RoadName = "Solomyanskiy", TrafficLevel = "High" },
                new TrafficData { RoadName = "Darnytskiy", TrafficLevel = "Medium" },
                new TrafficData { RoadName = "Obolonskiy", TrafficLevel = "Comfortable" }
            };

            _timer = new System.Timers.Timer(15000); 
            _timer.Elapsed += (sender, e) => SimulateTraffic();
            _timer.AutoReset = true;
            _timer.Start();
        }

        public void SimulateTraffic()
        {
            var rnd = new Random();
            foreach (var data in _trafficData)
            {
                data.TrafficLevel = _trafficLevels[rnd.Next(_trafficLevels.Length)];
            }
        }

        public List<TrafficData> GetTrafficData()
        {
            return _trafficData;
        }
    }
}

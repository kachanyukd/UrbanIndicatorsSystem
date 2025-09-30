using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public interface ITrafficService
    {
        IEnumerable<TrafficData> GetTrafficForArea(string areaName);
    }
}

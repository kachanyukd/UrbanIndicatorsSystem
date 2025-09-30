using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public interface IAreaService
    {
        IEnumerable<Area> GetAreas();
    }
}

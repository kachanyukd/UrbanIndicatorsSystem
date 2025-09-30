using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public class AreaService : IAreaService
    {
        public IEnumerable<Area> GetAreas()
        {
            return new List<Area>
            {
                new Area { Name = "Печерський район" },
                new Area { Name = "Шевченківський район" },
                new Area { Name = "Голосіївський район" }
            };
        }
    }
}

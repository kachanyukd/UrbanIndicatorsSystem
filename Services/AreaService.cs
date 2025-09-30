using UrbanIndicatorsSystem.Models;
using System.Collections.Generic;

namespace UrbanIndicatorsSystem.Services
{
    public class AreaService : IAreaService
    {
        public List<Area> GetAreas()
        {
            return new List<Area>
            {
                new Area { Id = 1, Name = "Pechersk" },
                new Area { Id = 2, Name = "Shevchenkivskyi" },
                new Area { Id = 3, Name = "Holosiivskyi" },
                new Area { Id = 4, Name = "Solomyanskiy" },
                new Area { Id = 5, Name = "Darnytskiy" },
                new Area { Id = 6, Name = "Obolonskiy" }
            };
        }
    }
}

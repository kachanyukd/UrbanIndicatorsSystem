using Microsoft.EntityFrameworkCore;
using UrbanIndicatorsSystem.Models;

namespace UrbanIndicatorsSystem.Data
{
    public class TrafficDbContext : DbContext
    {
        public TrafficDbContext(DbContextOptions<TrafficDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }
        public DbSet<TrafficData> TrafficData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Area>().HasData(
                new Area { Id = 1, Name = "Shevchenkivskyi" },
                new Area { Id = 2, Name = "Pecherskyi" },
                new Area { Id = 3, Name = "Podilskyi" },
                new Area { Id = 4, Name = "Darnytskyi" },
                new Area { Id = 5, Name = "Holosiivskyi" },
                new Area { Id = 6, Name = "Solomianskyi" },
                new Area { Id = 7, Name = "Dniprovskiy" },
                new Area { Id = 8, Name = "Obolonskyi" },
                new Area { Id = 9, Name = "Sviatoshynskyi" },
                new Area { Id = 10, Name = "Desnianskyi" }
            );

            modelBuilder.Entity<TrafficData>().HasData(
                new TrafficData { Id = 1, AreaId = 1, RoadName = "Shevchenkivskyi", TrafficLevel = "Low",        Timestamp = new DateTime(2025, 1, 1, 12, 00, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 2, AreaId = 2, RoadName = "Pecherskyi",      TrafficLevel = "Moderate",  Timestamp = new DateTime(2025, 1, 1, 12, 05, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 3, AreaId = 3, RoadName = "Podilskyi",       TrafficLevel = "High",      Timestamp = new DateTime(2025, 1, 1, 12, 10, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 4, AreaId = 4, RoadName = "Darnytskyi",      TrafficLevel = "Comfortable",    Timestamp = new DateTime(2025, 1, 1, 12, 15, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 5, AreaId = 5, RoadName = "Holosiivskyi",    TrafficLevel = "Low", Timestamp = new DateTime(2025, 1, 1, 12, 20, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 6, AreaId = 6, RoadName = "Solomianskyi",    TrafficLevel = "Moderate",  Timestamp = new DateTime(2025, 1, 1, 12, 25, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 7, AreaId = 7, RoadName = "Dniprovskiy",     TrafficLevel = "High",      Timestamp = new DateTime(2025, 1, 1, 12, 30, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 8, AreaId = 8, RoadName = "Obolonskyi",      TrafficLevel = "Low",    Timestamp = new DateTime(2025, 1, 1, 12, 35, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 9, AreaId = 9, RoadName = "Sviatoshynskyi",  TrafficLevel = "Comfortable", Timestamp = new DateTime(2025, 1, 1, 12, 40, 00, DateTimeKind.Utc) },
                new TrafficData { Id = 10, AreaId = 10, RoadName = "Desnianskyi",   TrafficLevel = "High",        Timestamp = new DateTime(2025, 1, 1, 12, 45, 00, DateTimeKind.Utc) }
            );
        }
    }
}
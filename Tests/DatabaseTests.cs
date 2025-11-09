using Microsoft.EntityFrameworkCore;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Xunit;

namespace UrbanIndicatorsSystem.Tests.IntegrationTests
{
    public class DatabaseTests
    {
        [Fact]
        public async Task InMemoryDatabase_CanAddAndRetrieveData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrafficDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb1")
                .Options;

            using var context = new TrafficDbContext(options);

            var area = new Area { Name = "TestArea" };
            context.Areas.Add(area);
            await context.SaveChangesAsync();

            // Act
            var retrievedArea = await context.Areas.FirstOrDefaultAsync(a => a.Name == "TestArea");

            // Assert
            Assert.NotNull(retrievedArea);
            Assert.Equal("TestArea", retrievedArea.Name);
        }

        [Fact]
        public async Task SQLiteDatabase_CanAddAndRetrieveData()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrafficDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            using var context = new TrafficDbContext(options);
            await context.Database.OpenConnectionAsync();
            await context.Database.EnsureCreatedAsync();

            var area = new Area { Name = "SQLiteTestArea" };
            context.Areas.Add(area);
            await context.SaveChangesAsync();

            // Act
            var retrievedArea = await context.Areas.FirstOrDefaultAsync(a => a.Name == "SQLiteTestArea");

            // Assert
            Assert.NotNull(retrievedArea);
            Assert.Equal("SQLiteTestArea", retrievedArea.Name);
        }

        [Fact]
        public async Task TrafficData_HasCorrectRelationshipWithArea()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrafficDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb2")
                .Options;

            using var context = new TrafficDbContext(options);

            var area = new Area { Name = "RelationTestArea" };
            context.Areas.Add(area);
            await context.SaveChangesAsync();

            var traffic = new TrafficData
            {
                RoadName = "Test Road",
                TrafficLevel = "Medium",
                AreaId = area.Id,
                Timestamp = DateTime.UtcNow
            };
            context.TrafficData.Add(traffic);
            await context.SaveChangesAsync();

            // Act
            var retrievedTraffic = await context.TrafficData
                .Include(t => t.Area)
                .FirstOrDefaultAsync(t => t.RoadName == "Test Road");

            // Assert
            Assert.NotNull(retrievedTraffic);
            Assert.NotNull(retrievedTraffic.Area);
            Assert.Equal("RelationTestArea", retrievedTraffic.Area.Name);
        }

        [Fact]
        public async Task ConcurrentAccess_HandlesMultipleOperations()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<TrafficDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb3")
                .Options;

            // Act
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
            {
                int index = i;
                tasks.Add(Task.Run(async () =>
                {
                    using var context = new TrafficDbContext(options);
                    var area = new Area { Name = $"ConcurrentArea{index}" };
                    context.Areas.Add(area);
                    await context.SaveChangesAsync();
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            using var verifyContext = new TrafficDbContext(options);
            var count = await verifyContext.Areas.CountAsync();
            Assert.Equal(10, count);
        }
    }
}
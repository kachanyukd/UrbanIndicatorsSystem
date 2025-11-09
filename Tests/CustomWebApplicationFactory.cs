using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UrbanIndicatorsSystem.Data;

namespace UrbanIndicatorsSystem.Tests.IntegrationTests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            
            builder.ConfigureServices(services =>
            {
                // Remove ALL existing DbContext registrations including the implementation
                var descriptors = services.Where(d => 
                    d.ServiceType == typeof(DbContextOptions<TrafficDbContext>) ||
                    d.ServiceType == typeof(TrafficDbContext) ||
                    d.ImplementationType == typeof(TrafficDbContext)).ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                // Add InMemory database for testing
                services.AddDbContext<TrafficDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryTestDb");
                });
            });
            
            builder.UseEnvironment("Test");
        }
        
        protected override void ConfigureClient(HttpClient client)
        {
            using (var scope = Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<TrafficDbContext>();

                db.Database.EnsureCreated();

                // Seed test data
                SeedTestData(db);
            }
            
            base.ConfigureClient(client);
        }

        private void SeedTestData(TrafficDbContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var area1 = new UrbanIndicatorsSystem.Models.Area { Name = "TestArea1" };
            var area2 = new UrbanIndicatorsSystem.Models.Area { Name = "TestArea2" };

            context.Areas.AddRange(area1, area2);
            context.SaveChanges();

            context.TrafficData.AddRange(
                new UrbanIndicatorsSystem.Models.TrafficData
                {
                    RoadName = "Test Road 1",
                    TrafficLevel = "High",
                    AreaId = area1.Id,
                    Timestamp = DateTime.UtcNow
                },
                new UrbanIndicatorsSystem.Models.TrafficData
                {
                    RoadName = "Test Road 2",
                    TrafficLevel = "Low",
                    AreaId = area2.Id,
                    Timestamp = DateTime.UtcNow
                }
            );

            context.SaveChanges();
        }
    }
}
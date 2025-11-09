using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace UrbanIndicatorsSystem.Services
{
    public class TrafficUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TrafficUpdateService> _logger;
        private readonly IConfiguration _configuration;

        public TrafficUpdateService(
            IServiceProvider serviceProvider,
            ILogger<TrafficUpdateService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var enabled = _configuration.GetSection("TrafficUpdate").GetValue<bool>("Enabled");
            if (!enabled)
            {
                _logger.LogInformation("Traffic Update Service is disabled");
                return;
            }

            var intervalSeconds = _configuration.GetSection("TrafficUpdate").GetValue<int>("IntervalSeconds");
            _logger.LogInformation("Traffic Update Service started with {interval} seconds interval", intervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var trafficService = scope.ServiceProvider.GetRequiredService<ITrafficService>();
                        await trafficService.SimulateTraffic();
                        _logger.LogInformation("Traffic data updated at {time}", DateTimeOffset.Now);
                    }

                    await Task.Delay(TimeSpan.FromSeconds(intervalSeconds), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while updating traffic data");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.LogInformation("Traffic Update Service stopped");
        }
    }
}
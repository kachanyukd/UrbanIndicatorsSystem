using System.Text.Json;
using StackExchange.Redis;
using UrbanIndicatorsSystem.Data;
using UrbanIndicatorsSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace UrbanIndicatorsSystem.Services
{
    public class TrafficService : ITrafficService
    {
        private readonly TrafficDbContext _db;
        private readonly IDatabase _redis;
        private readonly string _cacheKey = "traffic_data";

        private readonly string[] _trafficLevels = new[]
        {
            "Low",
            "Comfortable",
            "Moderate",
            "Medium",
            "High"
        };

        public TrafficService(TrafficDbContext db, IConnectionMultiplexer redis)
        {
            _db = db;
            _redis = redis.GetDatabase();
        }

        public async Task SimulateTraffic()
        {
            var trafficData = await _db.TrafficData.ToListAsync();
            var rnd = new Random();

            foreach (var data in trafficData)
            {
                data.TrafficLevel = _trafficLevels[rnd.Next(_trafficLevels.Length)];
                data.Timestamp = DateTime.UtcNow; 
            }

            await _db.SaveChangesAsync();
            await _redis.StringSetAsync(_cacheKey, JsonSerializer.Serialize(trafficData), TimeSpan.FromSeconds(15));
        }

        public async Task<List<TrafficData>> GetTrafficData()
        {
            await SimulateTraffic();

            if (await _redis.KeyExistsAsync(_cacheKey))
            {
                var cached = await _redis.StringGetAsync(_cacheKey);
                return JsonSerializer.Deserialize<List<TrafficData>>(cached!)!;
            }

            var freshData = await _db.TrafficData.ToListAsync();
            await _redis.StringSetAsync(_cacheKey, JsonSerializer.Serialize(freshData), TimeSpan.FromSeconds(15));
            return freshData;
        }
    }
}
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
        private readonly IDatabase? _redis;
        private readonly string _cacheKey = "traffic_data";

        private readonly string[] _trafficLevels = new[]
        {
            "Low",
            "Comfortable",
            "Moderate",
            "Medium",
            "High"
        };

        public TrafficService(TrafficDbContext db, IConnectionMultiplexer? redis = null)
        {
            _db = db;
            _redis = redis?.GetDatabase();
        }

        public async Task SimulateTraffic()
        {
            var trafficData = await _db.TrafficData.ToListAsync();
            var rnd = new Random();

            // Якщо немає даних, створимо нові
            if (!trafficData.Any())
            {
                var area = await _db.Areas.FirstOrDefaultAsync();
                if (area == null)
                {
                    area = new Area { Name = "Downtown" };
                    _db.Areas.Add(area);
                    await _db.SaveChangesAsync();
                }

                var roads = new[] { "Main Street", "Broadway", "5th Avenue", "Park Lane", "Market Street" };
                foreach (var road in roads)
                {
                    _db.TrafficData.Add(new TrafficData
                    {
                        RoadName = road,
                        TrafficLevel = _trafficLevels[rnd.Next(_trafficLevels.Length)],
                        AreaId = area.Id,
                        Timestamp = DateTime.UtcNow
                    });
                }
                await _db.SaveChangesAsync();
                trafficData = await _db.TrafficData.ToListAsync();
            }
            else
            {
                // Оновлюємо існуючі дані
                foreach (var data in trafficData)
                {
                    data.TrafficLevel = _trafficLevels[rnd.Next(_trafficLevels.Length)];
                    data.Timestamp = DateTime.UtcNow;
                }
                await _db.SaveChangesAsync();
            }

            // Кешуємо якщо Redis доступний
            if (_redis != null)
            {
                await _redis.StringSetAsync(_cacheKey, JsonSerializer.Serialize(trafficData), TimeSpan.FromSeconds(15));
            }
        }

        public async Task<List<TrafficData>> GetTrafficData()
        {
            // Спробуємо отримати з кешу якщо Redis доступний
            if (_redis != null && await _redis.KeyExistsAsync(_cacheKey))
            {
                var cached = await _redis.StringGetAsync(_cacheKey);
                if (!cached.IsNullOrEmpty)
                {
                    return JsonSerializer.Deserialize<List<TrafficData>>(cached!)!;
                }
            }

            // Отримуємо з бази з включенням Area
            var freshData = await _db.TrafficData
                .Include(t => t.Area)
                .OrderByDescending(t => t.Timestamp)
                .ToListAsync();

            // Якщо даних немає, створюємо їх
            if (!freshData.Any())
            {
                await SimulateTraffic();
                freshData = await _db.TrafficData
                    .Include(t => t.Area)
                    .OrderByDescending(t => t.Timestamp)
                    .ToListAsync();
            }

            // Кешуємо якщо Redis доступний
            if (_redis != null)
            {
                await _redis.StringSetAsync(_cacheKey, JsonSerializer.Serialize(freshData), TimeSpan.FromSeconds(15));
            }

            return freshData;
        }
    }
}
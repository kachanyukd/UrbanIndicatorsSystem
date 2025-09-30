namespace UrbanIndicatorsSystem.Models
{
    public class TrafficData
    {
        public int Id { get; set; }
        public string RoadName { get; set; } = string.Empty;
        public string TrafficLevel { get; set; } = string.Empty;
        public int AreaId { get; set; }
    }
}

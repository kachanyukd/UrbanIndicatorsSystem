export default function TrafficCard({ data }) {
  const colorMap = {
    Low: "green",
    Comfortable: "lightgreen",
    Moderate: "orange",
    Medium: "orangered",
    High: "red"
  };

  return (
    <div className="traffic-card">
      <h3>{data.roadName}</h3>
      <p style={{ color: colorMap[data.trafficLevel] }}>
        {data.trafficLevel}
      </p>
    </div>
  );
}
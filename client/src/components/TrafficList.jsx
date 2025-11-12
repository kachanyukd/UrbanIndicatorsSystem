import { useEffect, useState } from "react";
import { getTraffic } from "../api/trafficApi";
import TrafficCard from "./TrafficCard";

export default function TrafficList() {
  const [traffic, setTraffic] = useState([]);

  useEffect(() => {
    getTraffic().then(res => setTraffic(res.data));
  }, []);

  return (
    <div className="traffic-list">
      {traffic.map(item => (
        <TrafficCard key={item.id} data={item} />
      ))}
    </div>
  );
}

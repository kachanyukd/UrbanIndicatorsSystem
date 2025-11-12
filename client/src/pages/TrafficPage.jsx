import { useState, useEffect } from "react";
import TrafficList from "../components/TrafficList";
import TrafficSearchPanel from "../components/TrafficSearchPanel";
import { getTraffic } from "../api/trafficApi";

export default function TrafficPage() {
  const [traffic, setTraffic] = useState([]);

  useEffect(() => {
    getTraffic().then(res => setTraffic(res.data));
  }, []);

  return (
    <div>
      <h1>Urban Traffic Dashboard</h1>
      <TrafficSearchPanel setTraffic={setTraffic} />
      <TrafficList traffic={traffic} />
    </div>
  );
}

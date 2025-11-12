import { useState } from "react";
import { searchTraffic } from "../api/trafficApi";

export default function TrafficSearchPanel({ setTraffic }) {
  const [filters, setFilters] = useState({
    roadName: "",
    trafficLevel: ""
  });

  const handleSearch = async () => {
    const params = new URLSearchParams(filters).toString();
    const res = await searchTraffic(params);
    setTraffic(res.data);
  };

  return (
    <div className="search-panel">
      <input
        placeholder="Road name"
        value={filters.roadName}
        onChange={e => setFilters({ ...filters, roadName: e.target.value })}
      />
      <select
        value={filters.trafficLevel}
        onChange={e => setFilters({ ...filters, trafficLevel: e.target.value })}
      >
        <option value="">All levels</option>
        <option value="Low">Low</option>
        <option value="Comfortable">Comfortable</option>
        <option value="Moderate">Moderate</option>
        <option value="Medium">Medium</option>
        <option value="High">High</option>
      </select>
      <button onClick={handleSearch}>Search</button>
    </div>
  );
}
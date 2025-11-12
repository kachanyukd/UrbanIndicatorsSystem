// trafficChart.jsx
import React from "react";
import { Line } from "react-chartjs-2";

const TrafficChart = () => {
  const data = {
    labels: ["Пн", "Вт", "Ср", "Чт", "Пт", "Сб", "Нд"],
    datasets: [
      {
        label: "Рівень трафіку",
        data: [30, 50, 40, 60, 70, 45, 55],
        fill: false,
        borderColor: "rgba(75,192,192,1)",
        tension: 0.1,
      },
    ],
  };

  return (
    <div style={{ width: "80%", margin: "auto" }}>
      <h3>Статистика трафіку</h3>
      <Line data={data} />
    </div>
  );
};

export default TrafficChart;
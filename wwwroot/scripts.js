let autoUpdateEnabled = true;
let currentChart = null;

async function loadTrafficData() {
    if (!autoUpdateEnabled) return; 

    try {
        const response = await fetch('/api/traffic');
        const data = await response.json();
        renderTraffic(data);
    } catch (err) {
        console.error("Error loading traffic data:", err);
    }
}

function renderTraffic(data) {
    const container = document.getElementById("traffic-container");
    container.innerHTML = "";

    data.forEach(item => {
        const card = document.createElement("div");
        card.className = "traffic-card";

        const name = document.createElement("h2");
        name.textContent = item.roadName;

        const level = document.createElement("p");
        level.textContent = `Traffic level: ${item.trafficLevel}`;
        level.className = "traffic-level";
        level.style.color = getTrafficColor(item.trafficLevel);

        card.appendChild(name);
        card.appendChild(level);
        container.appendChild(card);
    });
}

function getTrafficColor(level) {
    switch (level.toLowerCase()) {
        case "low": return "green";
        case "comfortable": return "limegreen";
        case "moderate": return "orange";
        case "medium": return "orangered";
        case "high": return "red";
        default: return "black";
    }
}

function updateClock() {
    const clock = document.getElementById("clock");
    const now = new Date();
    const hours = String(now.getHours()).padStart(2, "0");
    const minutes = String(now.getMinutes()).padStart(2, "0");
    const seconds = String(now.getSeconds()).padStart(2, "0");
    clock.textContent = `${hours}:${minutes}:${seconds}`;
}

function showStatistics(data) {
    const chartContainer = document.getElementById("trafficChart");
    chartContainer.style.display = "block";

    if (currentChart) {
        currentChart.destroy(); 
    }

    const labels = data.map(item => item.roadName);
    const trafficLevels = data.map(item => trafficLevelToNumber(item.trafficLevel));

    currentChart = new Chart(chartContainer, {
        type: 'bar',
        data: {
            labels: labels,
            datasets: [{
                label: 'Traffic Level',
                data: trafficLevels,
                backgroundColor: 'rgba(54, 162, 235, 0.7)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: { y: { beginAtZero: true, max: 5 } }
        }
    });
}

function trafficLevelToNumber(level) {
    switch (level.toLowerCase()) {
        case "low": return 1;
        case "comfortable": return 2;
        case "moderate": return 3;
        case "medium": return 4;
        case "high": return 5;
        default: return 0;
    }
}

document.getElementById("show-stats-btn").addEventListener("click", async () => {
    try {
        const response = await fetch('/api/traffic');
        const data = await response.json();
        showStatistics(data);

        document.getElementById("show-stats-btn").style.display = "none";
        document.getElementById("hide-stats-btn").style.display = "inline-block";
    } catch (err) {
        console.error("Error loading statistics:", err);
    }
});

document.getElementById("hide-stats-btn").addEventListener("click", () => {
    document.getElementById("trafficChart").style.display = "none";
    document.getElementById("show-stats-btn").style.display = "inline-block";
    document.getElementById("hide-stats-btn").style.display = "none";
});

setInterval(updateClock, 1000);
updateClock();

loadTrafficData();
setInterval(loadTrafficData, 15000); 
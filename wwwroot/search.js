document.getElementById("search-btn").addEventListener("click", () => {
    const panel = document.getElementById("search-panel");
    panel.style.display = panel.style.display === "none" ? "block" : "none";
});

document.getElementById("apply-search-btn").addEventListener("click", async () => {
    const roadName = document.getElementById("roadName").value.trim();
    const startsWith = document.getElementById("startsWith").value.trim();
    const endsWith = document.getElementById("endsWith").value.trim();
    const trafficLevel = document.getElementById("trafficLevel").value;
    const from = document.getElementById("from").value;
    const to = document.getElementById("to").value;

    const params = new URLSearchParams();
    if (roadName) params.set("roadName", roadName);
    if (startsWith) params.set("startsWith", startsWith);
    if (endsWith) params.set("endsWith", endsWith);
    if (trafficLevel) params.set("trafficLevel", trafficLevel);
    if (from) params.set("from", from);
    if (to) params.set("to", to);

    const url = `/api/traffic/search?${params.toString()}`;
    console.log("[SEARCH] URL →", url);

    try {
        const response = await fetch(url);
        if (!response.ok) throw new Error(`API error: ${response.status}`);

        const data = await response.json();
        const container = document.getElementById("traffic-container");
        container.innerHTML = "";

        if (!data || data.length === 0) {
            container.innerHTML = `<p>No results found</p>`;
            return;
        }

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

    } catch (err) {
        console.error("Search failed:", err);
        alert("Search failed — check API or filters");
    }
});

document.getElementById("reset-search-btn").addEventListener("click", () => {
    document.getElementById("roadName").value = "";
    document.getElementById("startsWith").value = "";
    document.getElementById("endsWith").value = "";
    document.getElementById("trafficLevel").value = "";
    document.getElementById("from").value = "";
    document.getElementById("to").value = "";

    loadTrafficData(); 
});
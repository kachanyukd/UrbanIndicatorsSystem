import axios from "axios";

const API = axios.create({
  baseURL: "http://localhost:5000/api", // твій ASP.NET Core бекенд
});

export const getTraffic = () => API.get("/traffic");
export const searchTraffic = (params) => API.get(`/traffic/search?${params}`);
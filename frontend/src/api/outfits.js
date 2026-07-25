import { apiGet, apiPost } from "./client";

export function generateOutfit(poseId, garmentIds) {
  return apiPost("/Outfit/generate", { poseId, garmentIds });
}

export function getGeneratedOutfit(id) {
  return apiGet(`/Outfit/${id}`);
}
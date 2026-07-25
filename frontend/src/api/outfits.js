import { apiDelete, apiGet, apiPost } from "./client";

export function generateOutfit(poseId, garmentIds) {
  return apiPost("/Outfit/generate", { poseId, garmentIds });
}

export function getGeneratedOutfit(id) {
  return apiGet(`/Outfit/${id}`);
}

export function getAllOutfits() {
  return apiGet("/Outfit");
}

export function toggleFavoriteOutfit(id) {
  return apiPost(`/Outfit/${id}/favorite`, {});
}

export function deleteOutfit(id){
  return apiDelete(`/Outfit/${id}`);
}
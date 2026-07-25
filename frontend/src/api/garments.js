import { apiGet, apiPost, apiDelete } from "./client";

export function getGarments() {
  return apiGet("/Garment");
}

export function createGarment(garmentData) {
  return apiPost("/Garment", garmentData);
}

export function deleteGarment(id) {
  return apiDelete(`/Garment/${id}`);
}
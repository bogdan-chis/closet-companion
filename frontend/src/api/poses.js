import { apiGet, apiPost, apiDelete } from "./client";

export function getPoses() {
  return apiGet("/PosePhoto");
}

export function createPose(poseData) {
  return apiPost("/PosePhoto", poseData);
}

export function deletePose(id) {
  return apiDelete(`/PosePhoto/${id}`);
}
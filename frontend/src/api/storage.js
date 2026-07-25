import { apiPostForm } from "./client";

export async function uploadFile(file, folder = "garments") {
  const formData = new FormData();
  formData.append("file", file);
  formData.append("folder", folder);

  const result = await apiPostForm("/Storage/upload", formData);
  return result.imageUrl;
}
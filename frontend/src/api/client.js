const BASE_URL = "http://localhost:5232";

async function handleResponse(response) {
  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || `Request failed with status ${response.status}`);
  }
  // Some endpoints (e.g. DELETE) may return no content
  const text = await response.text();
  return text ? JSON.parse(text) : null;
}

export async function apiGet(path) {
  const response = await fetch(`${BASE_URL}${path}`);
  return handleResponse(response);
}

export async function apiPost(path, body) {
  const response = await fetch(`${BASE_URL}${path}`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(body),
  });
  return handleResponse(response);
}

export async function apiPostForm(path, formData) {
  const response = await fetch(`${BASE_URL}${path}`, {
    method: "POST",
    body: formData, // no Content-Type header — browser sets multipart boundary
  });
  return handleResponse(response);
}

export async function apiDelete(path) {
  const response = await fetch(`${BASE_URL}${path}`, { method: "DELETE" });
  return handleResponse(response);
}
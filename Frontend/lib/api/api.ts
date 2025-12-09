const BASE_URL = "http://localhost:5062/api";

function getAuthHeaders(): Record<string, string> {
  const token =
    typeof window !== "undefined" ? localStorage.getItem("token") : null;

  const headers: Record<string, string> = {
    "Content-Type": "application/json",
  };

  // Agrega Authorization SOLO si existe
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  return headers;
}

export const api = {
  async get(url: string) {
    return fetch(`${BASE_URL}${url}`, {
      method: "GET",
      headers: getAuthHeaders(),
    });
  },

  async post(url: string, body?: any) {
    return fetch(`${BASE_URL}${url}`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: body ? JSON.stringify(body) : undefined,
    });
  },

  async put(url: string, body?: any) {
    return fetch(`${BASE_URL}${url}`, {
      method: "PUT",
      headers: getAuthHeaders(),
      body: body ? JSON.stringify(body) : undefined,
    });
  },

  async delete(url: string) {
    return fetch(`${BASE_URL}${url}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });
  },
};

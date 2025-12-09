import { api } from "./api";
import type { LoginRequest, RegisterRequest, UserResponse } from "@/types/auth";

export const authApi = {
  async login(data: LoginRequest): Promise<UserResponse> {
    const res = await api.post("/auth/login", data);

    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || "Error al iniciar sesión");
    }

    return await res.json() as UserResponse;
  },

  async register(data: RegisterRequest): Promise<UserResponse> {
    const res = await api.post("/auth/register", data);

    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || "Error al registrar taller");
    }

    return await res.json() as UserResponse;
  },
};

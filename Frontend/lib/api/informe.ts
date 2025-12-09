import { api } from "./api";
import type { Informe } from "@/types/informe";

export const informesApi = {
  async getInforme(): Promise<Informe> {
    const res = await api.get("/informes");

    if (!res.ok) {
      const err = await res.json().catch(() => ({}));
      throw new Error(err.message || "Error al obtener informe");
    }

    return await res.json();
  }
};


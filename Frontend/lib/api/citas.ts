import { api } from "./api";
import type { Cita, CreateCita, UpdateCita } from "@/types/cita";

export const citasApi = {
  async getAll(): Promise<Cita[]> {
    const res = await api.get("/citas");
    if (!res.ok) throw new Error("Error obteniendo citas");
    return res.json();
  },

  async getById(id: string): Promise<Cita> {
    const res = await api.get(`/citas/${id}`);
    if (!res.ok) throw new Error("No se pudo obtener la cita");
    return res.json();
  },

  async create(data: CreateCita): Promise<Cita> {
    const res = await api.post("/citas", data);
    if (!res.ok) throw new Error("Error creando la cita");
    return res.json();
  },

  async update(id: string, data: UpdateCita): Promise<Cita> {
    const res = await api.put(`/citas/${id}`, data);
    if (!res.ok) throw new Error("Error actualizando la cita");
    return res.json();
  },

  async delete(id: string): Promise<boolean> {
    const res = await api.delete(`/citas/${id}`);
    if (!res.ok) throw new Error("Error eliminando la cita");
    return true;
  },
};

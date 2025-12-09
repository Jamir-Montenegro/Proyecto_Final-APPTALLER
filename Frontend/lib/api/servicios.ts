import { api } from "./api";
import {
  Servicio,
  CreateServicioRequest,
  UpdateServicioRequest,
} from "@/types/servicio";

/**
 * Servicio API para manejar los servicios (historial de trabajos)
 */
export const serviciosApi = {
  /** Obtener todos los servicios del taller */
  async getAll(): Promise<Servicio[]> {
    const res = await api.get("/servicios");
    if (!res.ok) throw new Error("Error obteniendo servicios");
    return res.json();
  },

  /** Obtener un servicio por ID */
  async getById(id: string): Promise<Servicio> {
    const res = await api.get(`/servicios/${id}`);
    if (!res.ok) throw new Error("Servicio no encontrado");
    return res.json();
  },

  /** Crear un nuevo servicio */
  async create(data: CreateServicioRequest): Promise<Servicio> {
    const res = await api.post("/servicios", data);
    if (!res.ok) throw new Error("Error creando servicio");
    return res.json();
  },

  /** Actualizar un servicio */
  async update(id: string, data: UpdateServicioRequest): Promise<Servicio> {
    const res = await api.put(`/servicios/${id}`, data);
    if (!res.ok) throw new Error("Error actualizando servicio");
    return res.json();
  },

  /** Eliminar un servicio */
  async remove(id: string): Promise<boolean> {
    const res = await api.delete(`/servicios/${id}`);
    if (!res.ok) throw new Error("Error eliminando servicio");
    return true;
  },
};

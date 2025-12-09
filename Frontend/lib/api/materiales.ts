import { api } from "./api";
import {
  Material,
  CreateMaterialRequest,
  UpdateMaterialRequest,
} from "@/types/material";

/**
 * API de materiales (inventario del taller)
 */
export const materialesApi = {
  /** Obtener todos los materiales */
  async getAll(): Promise<Material[]> {
    const res = await api.get("/materiales");
    if (!res.ok) throw new Error("Error obteniendo materiales");
    return res.json();
  },

  /** Obtener un solo material por ID */
  async getById(id: string): Promise<Material> {
    const res = await api.get(`/materiales/${id}`);
    if (!res.ok) throw new Error("Material no encontrado");
    return res.json();
  },

  /** Crear material nuevo */
  async create(data: CreateMaterialRequest): Promise<Material> {
    const res = await api.post("/materiales", data);
    if (!res.ok) throw new Error("Error creando material");
    return res.json();
  },

  /** Actualizar material existente */
  async update(id: string, data: UpdateMaterialRequest): Promise<Material> {
    const res = await api.put(`/materiales/${id}`, data);
    if (!res.ok) throw new Error("Error actualizando material");
    return res.json();
  },

  /** Eliminar material */
  async remove(id: string): Promise<boolean> {
    const res = await api.delete(`/materiales/${id}`);
    if (!res.ok) throw new Error("Error eliminando material");
    return true;
  },
};

import { api } from "./api";
import {
  Vehiculo,
  CreateVehiculoRequest,
  UpdateVehiculoRequest,
} from "@/types/vehiculo";

const BASE_URL = "/vehiculos";

export const vehiculosApi = {
  async getAll(): Promise<Vehiculo[]> {
    const res = await api.get(BASE_URL);
    if (!res.ok) throw new Error("Error al obtener vehículos");
    return res.json();
  },

  async getById(id: string): Promise<Vehiculo> {
    const res = await api.get(`${BASE_URL}/${id}`);
    if (!res.ok) throw new Error("Error al obtener vehículo");
    return res.json();
  },

  async create(data: CreateVehiculoRequest): Promise<Vehiculo> {
    const res = await api.post(BASE_URL, data);
    if (!res.ok) throw new Error("Error al crear vehículo");
    return res.json();
  },

  async update(id: string, data: UpdateVehiculoRequest): Promise<Vehiculo> {
    const res = await api.put(`${BASE_URL}/${id}`, data);
    if (!res.ok) throw new Error("Error al actualizar vehículo");
    return res.json();
  },

  async delete(id: string): Promise<boolean> {
    const res = await api.delete(`${BASE_URL}/${id}`);
    if (!res.ok) throw new Error("Error al eliminar vehículo");
    return true;
  },
};


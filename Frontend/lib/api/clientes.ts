import { api } from "./api";
import {
  Cliente,
  CreateClienteRequest,
  UpdateClienteRequest,
} from "@/types/cliente";

const BASE_URL = "/clientes";

/* =============================
   GET: Obtener todos los clientes
============================= */
export async function getClientes(): Promise<Cliente[]> {
  const res = await api.get(BASE_URL);
  if (!res.ok) throw new Error("Error al obtener clientes");
  return res.json();
}

/* =============================
   GET: Obtener cliente por ID
============================= */
export async function getClienteById(id: string): Promise<Cliente> {
  const res = await api.get(`${BASE_URL}/${id}`);
  if (!res.ok) throw new Error("Error al obtener cliente");
  return res.json();
}

/* =============================
   POST: Crear cliente
============================= */
export async function createCliente(
  data: CreateClienteRequest
): Promise<Cliente> {
  const res = await api.post(BASE_URL, data);
  if (!res.ok) throw new Error("Error al crear cliente");
  return res.json();
}

/* =============================
   PUT: Actualizar cliente
============================= */
export async function updateCliente(
  id: string,
  data: UpdateClienteRequest
): Promise<Cliente> {
  const res = await api.put(`${BASE_URL}/${id}`, data);
  if (!res.ok) throw new Error("Error al actualizar cliente");
  return res.json();
}

/* =============================
   DELETE: Eliminar cliente
============================= */
export async function deleteCliente(id: string): Promise<boolean> {
  const res = await api.delete(`${BASE_URL}/${id}`);
  if (!res.ok) throw new Error("Error al eliminar cliente");
  return true;
}


export const clientesApi = {
  getAll: getClientes,
  getById: getClienteById,
  create: createCliente,
  update: updateCliente,
  delete: deleteCliente,
};


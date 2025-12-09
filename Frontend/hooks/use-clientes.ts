import { useState } from "react";
import { clientesApi } from "@/lib/api/clientes";
import { Cliente, CreateClienteRequest, UpdateClienteRequest } from "@/types/cliente";

export function useClientes() {
  const [clientes, setClientes] = useState<Cliente[]>([]);

  // Obtener todos los clientes
  async function fetchClientes() {
    const data = await clientesApi.getAll();
    setClientes(data);
    return data;
  }

  // Obtener cliente por ID
  async function fetchCliente(id: string) {
    return await clientesApi.getById(id);
  }

  // Crear nuevo cliente
  async function createCliente(data: CreateClienteRequest) {
    const created = await clientesApi.create(data);
    setClientes(prev => [...prev, created]);
    return created;
  }

  // Actualizar cliente existente
  async function updateCliente(id: string, data: UpdateClienteRequest) {
    const updated = await clientesApi.update(id, data);
    setClientes(prev =>
      prev.map(c => (c.id === id ? updated : c))
    );
    return updated;
  }

  

  // Eliminar cliente
  async function deleteCliente(id: string) {
    await clientesApi.delete(id);
    setClientes(prev => prev.filter(c => c.id !== id));

    
  }

  return {
    clientes,
    fetchClientes,
    fetchCliente,
    createCliente,
    updateCliente,
    deleteCliente,
  };

  
}


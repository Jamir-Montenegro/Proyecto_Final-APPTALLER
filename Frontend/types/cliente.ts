export interface Cliente {
  id: string;
  nombre: string;
  telefono: string;
  email?: string;
  direccion?: string;
  cedula?: string;
}

export interface CreateClienteRequest {
  nombre: string;
  telefono: string;
  email?: string;
  direccion?: string;
  cedula?: string;
}

export interface UpdateClienteRequest {
  nombre?: string;
  telefono?: string;
  email?: string;
  direccion?: string;
  cedula?: string;
}

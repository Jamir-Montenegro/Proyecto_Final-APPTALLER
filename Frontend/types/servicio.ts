export interface Servicio {
  id: string;
  vehiculoId: string;
  fecha: string;
  descripcion: string;
  costo: number;
  mecanico: string;
  notas?: string;
}

export interface CreateServicioRequest {
  vehiculoId: string;
  fecha: string;
  descripcion: string;
  costo: number;
  mecanico: string;
  notas?: string;
}

export interface UpdateServicioRequest {
  vehiculoId?: string;
  fecha?: string;
  descripcion?: string;
  costo?: number;
  mecanico?: string;
  notas?: string;
}

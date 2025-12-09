export interface Cita {
  id: string;
  clienteId: string;
  clienteNombre: string;
  clienteCedula: string;
  fechaHora: string; // ISO string desde backend
  descripcion?: string;
  estado: string; // Pendiente, En Progreso, Completada, Cancelada
}

export interface CreateCita {
  clienteId: string;
  fechaHora: string;
  descripcion?: string;
}

export interface UpdateCita {
  clienteId?: string;
  fechaHora?: string;
  descripcion?: string;
  estado?: string;
}


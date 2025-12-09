export interface Vehiculo {
  id: string;
  clienteId: string;
  clienteNombre: string;
  clienteCedula: string;
  marca: string;
  modelo: string;
  anio: number;
  placa: string;
  color?: string;
  vin?: string;
}

export interface CreateVehiculoRequest {
  clienteId: string;
  marca: string;
  modelo: string;
  anio: number;
  placa: string;
  color?: string;
  vin?: string;
}

export interface UpdateVehiculoRequest {
  clienteId?: string;
  marca?: string;
  modelo?: string;
  anio?: number;
  placa?: string;
  color?: string;
  vin?: string;
}

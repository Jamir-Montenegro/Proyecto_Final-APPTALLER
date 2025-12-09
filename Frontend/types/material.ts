export interface Material {
  id: string;
  nombre: string;
  descripcion?: string;
  cantidad: number;
  umbralBajo: number;
  precioUnitario: number;
  categoria?: string;
  proveedor?: string;
}

export interface CreateMaterialRequest {
  nombre: string;
  descripcion?: string;
  cantidad: number;
  umbralBajo: number;
  precioUnitario: number;
  categoria?: string;
  proveedor?: string;
}

export interface UpdateMaterialRequest {
  nombre?: string;
  descripcion?: string;
  cantidad?: number;
  umbralBajo?: number;
  precioUnitario?: number;
  categoria?: string;
  proveedor?: string;
}

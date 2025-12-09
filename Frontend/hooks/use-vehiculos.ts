import { useEffect, useState } from "react";
import { vehiculosApi } from "@/lib/api/vehiculos";
import {
  Vehiculo,
  CreateVehiculoRequest,
  UpdateVehiculoRequest,
} from "@/types/vehiculo";

export function useVehiculos() {
  const [vehiculos, setVehiculos] = useState<Vehiculo[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  //  Obtener todos los vehículos
  async function cargarVehiculos() {
    try {
      setLoading(true);
      const data = await vehiculosApi.getAll();
      setVehiculos(data);
    } catch (err: any) {
      setError(err.message || "Error al cargar vehículos");
    } finally {
      setLoading(false);
    }
  }

  //  Crear vehículo
  async function crearVehiculo(data: CreateVehiculoRequest) {
    const nuevo = await vehiculosApi.create(data);
    setVehiculos((prev) => [...prev, nuevo]);
    return nuevo;
  }

  //  Actualizar vehículo
  async function actualizarVehiculo(id: string, data: UpdateVehiculoRequest) {
    const actualizado = await vehiculosApi.update(id, data);
    setVehiculos((prev) =>
      prev.map((v) => (v.id === id ? actualizado : v))
    );
    return actualizado;
  }

  //  Eliminar vehículo
  async function eliminarVehiculo(id: string) {
    await vehiculosApi.delete(id);
    setVehiculos((prev) => prev.filter((v) => v.id !== id));
  }

  // Cargar al iniciar
  useEffect(() => {
    cargarVehiculos();
  }, []);

  return {
    vehiculos,
    loading,
    error,
    cargarVehiculos,
    crearVehiculo,
    actualizarVehiculo,
    eliminarVehiculo,
  };
}

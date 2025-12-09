"use client";

import { useState, useEffect } from "react";
import { serviciosApi } from "@/lib/api/servicios";
import {
  Servicio,
  CreateServicioRequest,
  UpdateServicioRequest,
} from "@/types/servicio";

export function useServicios() {
  const [servicios, setServicios] = useState<Servicio[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);


  // Cargar todos los servicios

  const loadServicios = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await serviciosApi.getAll();
      setServicios(data);
    } catch (err: any) {
      setError(err.message ?? "Error al cargar servicios");
    } finally {
      setLoading(false);
    }
  };


  // Crear un servicio

  const createServicio = async (data: CreateServicioRequest) => {
    try {
      setLoading(true);
      setError(null);
      const nuevo = await serviciosApi.create(data);
      setServicios((prev) => [...prev, nuevo]);
      return nuevo;
    } catch (err: any) {
      setError(err.message ?? "Error al crear servicio");
      throw err;
    } finally {
      setLoading(false);
    }
  };


  // Actualizar servicio

  const updateServicio = async (id: string, data: UpdateServicioRequest) => {
    try {
      setLoading(true);
      setError(null);
      const actualizado = await serviciosApi.update(id, data);

      setServicios((prev) =>
        prev.map((s) => (s.id === id ? actualizado : s))
      );

      return actualizado;
    } catch (err: any) {
      setError(err.message ?? "Error al actualizar servicio");
      throw err;
    } finally {
      setLoading(false);
    }
  };


  // Eliminar servicio

  const deleteServicio = async (id: string) => {
    try {
      setLoading(true);
      setError(null);
      await serviciosApi.remove(id);
      setServicios((prev) => prev.filter((s) => s.id !== id));
      return true;
    } catch (err: any) {
      setError(err.message ?? "Error al eliminar servicio");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadServicios();
  }, []);

  return {
    servicios,
    loading,
    error,
    loadServicios,
    createServicio,
    updateServicio,
    deleteServicio,
  };
}

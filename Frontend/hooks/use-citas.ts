"use client";

import { useState, useEffect } from "react";
import { citasApi } from "@/lib/api/citas";
import {
  Cita,
  CreateCita,
  UpdateCita,
} from "@/types/cita";

export function useCitas() {
  const [citas, setCitas] = useState<Cita[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Obtener todas las citas al cargar
  const loadCitas = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await citasApi.getAll();
      setCitas(data);
    } catch (err: any) {
      setError(err.message ?? "Error al cargar citas");
    } finally {
      setLoading(false);
    }
  };

  // Crear cita
  const createCita = async (data: CreateCita) => {
    try {
      setLoading(true);
      setError(null);
      const newCita = await citasApi.create(data);
      setCitas((prev) => [...prev, newCita]);
      return newCita;
    } catch (err: any) {
      setError(err.message ?? "Error al crear cita");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  // Actualizar cita
  const updateCita = async (id: string, data: UpdateCita) => {
    try {
      setLoading(true);
      setError(null);
      const updated = await citasApi.update(id, data);
      setCitas((prev) =>
        prev.map((c) => (c.id === id ? updated : c))
      );
      return updated;
    } catch (err: any) {
      setError(err.message ?? "Error al actualizar cita");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  // Eliminar cita
  const deleteCita = async (id: string) => {
    try {
      setLoading(true);
      setError(null);
      await citasApi.delete(id);
      setCitas((prev) => prev.filter((c) => c.id !== id));
      return true;
    } catch (err: any) {
      setError(err.message ?? "Error al eliminar cita");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadCitas();
  }, []);

  return {
    citas,
    loading,
    error,
    loadCitas,
    createCita,
    updateCita,
    deleteCita,
  };
}

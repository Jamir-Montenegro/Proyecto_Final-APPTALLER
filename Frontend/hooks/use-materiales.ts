"use client";

import { useState, useEffect } from "react";
import { materialesApi } from "@/lib/api/materiales";
import {
  Material,
  CreateMaterialRequest,
  UpdateMaterialRequest,
} from "@/types/material";

export function useMateriales() {
  const [materiales, setMateriales] = useState<Material[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Obtener materiales
  const loadMateriales = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await materialesApi.getAll();
      setMateriales(data);
    } catch (err: any) {
      setError(err.message ?? "Error al cargar materiales");
    } finally {
      setLoading(false);
    }
  };

  // Crear material
  const createMaterial = async (data: CreateMaterialRequest) => {
    try {
      setLoading(true);
      setError(null);
      const newMat = await materialesApi.create(data);
      setMateriales((prev) => [...prev, newMat]);
      return newMat;
    } catch (err: any) {
      setError(err.message ?? "Error al crear material");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  // Actualizar material
  const updateMaterial = async (id: string, data: UpdateMaterialRequest) => {
    try {
      setLoading(true);
      setError(null);
      const updated = await materialesApi.update(id, data);
      setMateriales((prev) =>
        prev.map((m) => (m.id === id ? updated : m))
      );
      return updated;
    } catch (err: any) {
      setError(err.message ?? "Error al actualizar material");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  // Eliminar material
  const deleteMaterial = async (id: string) => {
    try {
      setLoading(true);
      setError(null);
      await materialesApi.remove(id); // 
      setMateriales((prev) => prev.filter((m) => m.id !== id));
      return true;
    } catch (err: any) {
      setError(err.message ?? "Error al eliminar material");
      throw err;
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadMateriales();
  }, []);

  return {
    materiales,
    loading,
    error,
    loadMateriales,
    createMaterial,
    updateMaterial,
    deleteMaterial,
  };
}

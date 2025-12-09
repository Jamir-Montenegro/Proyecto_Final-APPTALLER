"use client";

import { useState, useEffect } from "react";
import { informesApi } from "@/lib/api/informe";
import { Informe } from "@/types/informe";

export function useInforme() {
  const [informe, setInforme] = useState<Informe | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadInforme = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await informesApi.getInforme();
      setInforme(data);
    } catch (err: any) {
      setError(err.message ?? "Error al obtener informe");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadInforme();
  }, []);

  return {
    informe,
    loading,
    error,
    loadInforme,
  };
}

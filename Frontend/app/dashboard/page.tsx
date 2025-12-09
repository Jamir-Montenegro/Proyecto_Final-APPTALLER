"use client";

import { useEffect } from "react";

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Car, Users, Calendar, Package } from "lucide-react";

// Hooks 
import { useVehiculos } from "@/hooks/use-vehiculos";
import { useClientes } from "@/hooks/use-clientes";
import { useCitas } from "@/hooks/use-citas";
import { useMateriales } from "@/hooks/use-materiales";

export default function DashboardPage() {
  const { vehiculos, cargarVehiculos } = useVehiculos();
  const { clientes, fetchClientes } = useClientes();
  const { citas, loadCitas } = useCitas();
  const { materiales, loadMateriales } = useMateriales();

 
  useEffect(() => {
    cargarVehiculos();
    fetchClientes();
    loadCitas();
    loadMateriales();
  }, []);

 
  const totalAutos = vehiculos.length;
  const totalClientes = clientes.length;
  const citasPendientes = citas.filter((c) => c.estado === "Pendiente").length;
  const totalInventario = materiales.length;

  const stats = [
    {
      name: "Total Autos",
      value: totalAutos,
      icon: Car,
      color: "text-blue-500",
    },
    {
      name: "Total Clientes",
      value: totalClientes,
      icon: Users,
      color: "text-green-500",
    },
    {
      name: "Citas Pendientes",
      value: citasPendientes,
      icon: Calendar,
      color: "text-orange-500",
    },
    {
      name: "Items en Inventario",
      value: totalInventario,
      icon: Package,
      color: "text-purple-500",
    },
  ];

  return (
    <div className="space-y-6">
      {/* TITULO */}
      <div>
        <h1 className="text-3xl font-bold text-foreground">Dashboard</h1>
        <p className="text-muted-foreground">Resumen general del taller</p>
      </div>

      {/* TARJETAS DE ESTADISTICAS */}
      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {stats.map((stat) => (
          <Card key={stat.name}>
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">
                {stat.name}
              </CardTitle>
              <stat.icon className={`h-5 w-5 ${stat.color}`} />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{stat.value}</div>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* RESUMEN */}
      <Card>
        <CardHeader>
          <CardTitle>Resumen del Taller</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-muted-foreground">
            Usa el menú lateral para acceder a Autos, Clientes, Inventario, Historial y más.
          </p>
        </CardContent>
      </Card>
    </div>
  );
}


"use client";

import { useEffect, useState } from "react";

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import { Car, Users, Calendar, CheckCircle, Clock, XCircle } from "lucide-react";

import { useVehiculos } from "@/hooks/use-vehiculos";
import { useClientes } from "@/hooks/use-clientes";
import { useCitas } from "@/hooks/use-citas";

export default function InformePage() {
  const { vehiculos, cargarVehiculos } = useVehiculos();
  const { clientes, fetchClientes } = useClientes();
  const { citas, loadCitas } = useCitas();

  const [isReady, setIsReady] = useState(false);

  useEffect(() => {
    async function loadAll() {
      await Promise.all([
        cargarVehiculos(),
        fetchClientes(),
        loadCitas(),
      ]);

      setIsReady(true);
    }

    loadAll();
  }, []);

  if (!isReady) {
    return (
      <div className="p-10 text-center text-xl text-muted-foreground">
        Cargando informe...
      </div>
    );
  }

  // ---- ESTADÍSTICAS ----
  const totalVehiculos = vehiculos.length;
  const totalClientes = clientes.length;
  const totalCitas = citas.length;

  const citasCompletadas = citas.filter((c) => c.estado === "Completada").length;
  const citasPendientes = citas.filter((c) => c.estado === "Pendiente").length;
  const citasEnProgreso = citas.filter((c) => c.estado === "En Progreso").length;
  const citasCanceladas = citas.filter((c) => c.estado === "Cancelada").length;

  const tasaCompletadas =
    totalCitas > 0 ? Math.round((citasCompletadas / totalCitas) * 100) : 0;

  const promedioVehiculosPorCliente =
    totalClientes > 0 ? (totalVehiculos / totalClientes).toFixed(1) : "0";

  const stats = [
    {
      title: "Vehículos Registrados",
      value: totalVehiculos,
      icon: Car,
      description: "Total en el sistema",
      color: "bg-blue-500",
    },
    {
      title: "Clientes Registrados",
      value: totalClientes,
      icon: Users,
      description: "Total en el sistema",
      color: "bg-green-500",
    },
    {
      title: "Citas Solicitadas",
      value: totalCitas,
      icon: Calendar,
      description: "Total de citas",
      color: "bg-purple-500",
    },
    {
      title: "Citas Atendidas",
      value: citasCompletadas,
      icon: CheckCircle,
      description: "Completadas",
      color: "bg-emerald-500",
    },
    {
      title: "Citas Pendientes",
      value: citasPendientes,
      icon: Clock,
      description: "Por atender",
      color: "bg-yellow-500",
    },
    {
      title: "Citas Canceladas",
      value: citasCanceladas,
      icon: XCircle,
      description: "No realizadas",
      color: "bg-red-500",
    },
  ];

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold">Informe del Taller</h1>
        <p className="text-muted-foreground">Resumen general de las operaciones</p>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
        {stats.map((stat, index) => (
          <Card key={index} className="hover:shadow-lg transition-shadow duration-200">
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium">{stat.title}</CardTitle>
              <div className={`${stat.color} p-2 rounded-md`}>
                <stat.icon className="h-4 w-4 text-white" />
              </div>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{stat.value}</div>
              <p className="text-xs text-muted-foreground">{stat.description}</p>
            </CardContent>
          </Card>
        ))}
      </div>

      {/* RESUMEN */}
      <Card>
        <CardHeader>
          <CardTitle>Resumen de Operaciones</CardTitle>
          <CardDescription>Vista general del estado del taller</CardDescription>
        </CardHeader>

        <CardContent className="space-y-4">
          <div className="flex items-center justify-between p-4 border rounded-lg">
            <div>
              <p className="font-medium">Tasa de Citas Completadas</p>
              <p className="text-sm text-muted-foreground">Porcentaje de citas finalizadas</p>
            </div>
            <div className="text-2xl font-bold text-green-600">{tasaCompletadas}%</div>
          </div>

          <div className="flex items-center justify-between p-4 border rounded-lg">
            <div>
              <p className="font-medium">Vehículos por Cliente</p>
              <p className="text-sm text-muted-foreground">Promedio por cliente</p>
            </div>
            <div className="text-2xl font-bold text-blue-600">{promedioVehiculosPorCliente}</div>
          </div>

          <div className="flex items-center justify-between p-4 border rounded-lg">
            <div>
              <p className="font-medium">Citas en Progreso</p>
              <p className="text-sm text-muted-foreground">Actualmente siendo atendidas</p>
            </div>
            <div className="text-2xl font-bold text-yellow-600">{citasEnProgreso}</div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

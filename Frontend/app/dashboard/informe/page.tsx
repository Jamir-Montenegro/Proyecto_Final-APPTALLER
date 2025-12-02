"use client"

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Car, Users, Calendar, CheckCircle, Clock, XCircle } from 'lucide-react'

export default function InformePage() {
  // Datos simulados para el informe
  const stats = [
    {
      title: "Vehículos Registrados",
      value: "3",
      icon: Car,
      description: "Total en el sistema",
      color: "bg-blue-500",
    },
    {
      title: "Clientes Registrados",
      value: "3",
      icon: Users,
      description: "Total en el sistema",
      color: "bg-green-500",
    },
    {
      title: "Citas Solicitadas",
      value: "3",
      icon: Calendar,
      description: "Total de citas",
      color: "bg-purple-500",
    },
    {
      title: "Citas Atendidas",
      value: "1",
      icon: CheckCircle,
      description: "Completadas",
      color: "bg-emerald-500",
    },
    {
      title: "Citas Pendientes",
      value: "1",
      icon: Clock,
      description: "Por atender",
      color: "bg-yellow-500",
    },
    {
      title: "Citas Canceladas",
      value: "0",
      icon: XCircle,
      description: "No realizadas",
      color: "bg-red-500",
    },
  ]

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
            <div className="text-2xl font-bold text-green-600">33%</div>
          </div>
          <div className="flex items-center justify-between p-4 border rounded-lg">
            <div>
              <p className="font-medium">Vehículos por Cliente</p>
              <p className="text-sm text-muted-foreground">Promedio de autos por cliente</p>
            </div>
            <div className="text-2xl font-bold text-blue-600">1.0</div>
          </div>
          <div className="flex items-center justify-between p-4 border rounded-lg">
            <div>
              <p className="font-medium">Citas en Progreso</p>
              <p className="text-sm text-muted-foreground">Actualmente siendo atendidas</p>
            </div>
            <div className="text-2xl font-bold text-yellow-600">1</div>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Car, Users, Calendar, Package } from "lucide-react"

export default function DashboardPage() {
  const mockStats = [
    {
      name: "Total Autos",
      value: 12,
      icon: Car,
      color: "text-blue-500",
    },
    {
      name: "Total Clientes",
      value: 8,
      icon: Users,
      color: "text-green-500",
    },
    {
      name: "Citas Pendientes",
      value: 5,
      icon: Calendar,
      color: "text-orange-500",
    },
    {
      name: "Items en Inventario",
      value: 45,
      icon: Package,
      color: "text-purple-500",
    },
  ]

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-foreground">Dashboard</h1>
        <p className="text-muted-foreground">Bienvenido al sistema de gestión del taller</p>
      </div>

      <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
        {mockStats.map((stat) => (
          <Card key={stat.name}>
            <CardHeader className="flex flex-row items-center justify-between pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">{stat.name}</CardTitle>
              <stat.icon className={`h-5 w-5 ${stat.color}`} />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">{stat.value}</div>
            </CardContent>
          </Card>
        ))}
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Resumen</CardTitle>
        </CardHeader>
        <CardContent>
          <p className="text-muted-foreground">
            Utiliza el menú lateral para navegar entre las diferentes secciones del sistema de gestión del taller.
          </p>
        </CardContent>
      </Card>
    </div>
  )
}

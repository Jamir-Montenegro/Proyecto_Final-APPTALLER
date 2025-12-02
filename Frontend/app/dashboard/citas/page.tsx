"use client"

import type React from "react"

import { useState } from "react"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Plus, Pencil, Trash2 } from 'lucide-react'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Textarea } from "@/components/ui/textarea"

type Cita = {
  id: string
  fecha: string
  descripcion: string
  estado: string
  auto_id: string
  cliente_id: string
}

type Cliente = {
  id: string
  nombre: string
  cedula: string
}

export default function CitasPage() {
  const mockClientes: Cliente[] = [
    { id: "1", nombre: "Juan Pérez", cedula: "8-1234-1234" },
    { id: "2", nombre: "María García", cedula: "4-567-8910" },
    { id: "3", nombre: "Carlos López", cedula: "3-789-4561" },
  ]

  const [citas, setCitas] = useState<Cita[]>([
    {
      id: "1",
      fecha: "2025-01-15T10:00",
      descripcion: "Cambio de aceite y filtro",
      estado: "pendiente",
      auto_id: "",
      cliente_id: "1",
    },
    {
      id: "2",
      fecha: "2025-01-16T14:00",
      descripcion: "Revisión de frenos",
      estado: "en_progreso",
      auto_id: "",
      cliente_id: "2",
    },
    {
      id: "3",
      fecha: "2025-01-17T09:00",
      descripcion: "Alineación y balanceo",
      estado: "completada",
      auto_id: "",
      cliente_id: "3",
    },
  ])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingCita, setEditingCita] = useState<Cita | null>(null)
  const [formData, setFormData] = useState({
    fecha: "",
    descripcion: "",
    estado: "pendiente",
    auto_id: "",
    cliente_id: "",
  })

  const getClienteNombre = (clienteId: string) => {
    const cliente = mockClientes.find((c) => c.id === clienteId)
    return cliente ? `${cliente.nombre} (${cliente.cedula})` : "Sin asignar"
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (editingCita) {
      setCitas(citas.map((c) => (c.id === editingCita.id ? { ...editingCita, ...formData } : c)))
    } else {
      setCitas([...citas, { id: Date.now().toString(), ...formData }])
    }
    setIsDialogOpen(false)
    setEditingCita(null)
    setFormData({
      fecha: "",
      descripcion: "",
      estado: "pendiente",
      auto_id: "",
      cliente_id: "",
    })
  }

  const handleEdit = (cita: Cita) => {
    setEditingCita(cita)
    setFormData({
      fecha: cita.fecha,
      descripcion: cita.descripcion,
      estado: cita.estado,
      auto_id: cita.auto_id,
      cliente_id: cita.cliente_id,
    })
    setIsDialogOpen(true)
  }

  const handleDelete = (id: string) => {
    setCitas(citas.filter((c) => c.id !== id))
  }

  const getEstadoBadge = (estado: string) => {
    const colors = {
      pendiente: "bg-yellow-500/10 text-yellow-500",
      en_progreso: "bg-blue-500/10 text-blue-500",
      completada: "bg-green-500/10 text-green-500",
      cancelada: "bg-red-500/10 text-red-500",
    }
    return colors[estado as keyof typeof colors] || colors.pendiente
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Citas</h1>
          <p className="text-muted-foreground">Gestiona las citas del taller</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingCita(null)
                setFormData({
                  fecha: "",
                  descripcion: "",
                  estado: "pendiente",
                  auto_id: "",
                  cliente_id: "",
                })
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Cita
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingCita ? "Editar Cita" : "Agregar Nueva Cita"}</DialogTitle>
              <DialogDescription>Completa la información de la cita</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid gap-2">
                <Label htmlFor="cliente">Cliente</Label>
                <Select
                  value={formData.cliente_id}
                  onValueChange={(value) => setFormData({ ...formData, cliente_id: value })}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecciona un cliente" />
                  </SelectTrigger>
                  <SelectContent>
                    {mockClientes.map((cliente) => (
                      <SelectItem key={cliente.id} value={cliente.id}>
                        {cliente.nombre} ({cliente.cedula})
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
              <div className="grid gap-2">
                <Label htmlFor="fecha">Fecha y Hora</Label>
                <Input
                  id="fecha"
                  type="datetime-local"
                  value={formData.fecha}
                  onChange={(e) => setFormData({ ...formData, fecha: e.target.value })}
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="descripcion">Descripción</Label>
                <Textarea
                  id="descripcion"
                  value={formData.descripcion}
                  onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="estado">Estado</Label>
                <Select value={formData.estado} onValueChange={(value) => setFormData({ ...formData, estado: value })}>
                  <SelectTrigger>
                    <SelectValue />
                  </SelectTrigger>
                  <SelectContent>
                    <SelectItem value="pendiente">Pendiente</SelectItem>
                    <SelectItem value="en_progreso">En Progreso</SelectItem>
                    <SelectItem value="completada">Completada</SelectItem>
                    <SelectItem value="cancelada">Cancelada</SelectItem>
                  </SelectContent>
                </Select>
              </div>
              <Button type="submit" className="w-full">
                {editingCita ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Citas</CardTitle>
          <CardDescription>Todas las citas programadas en el sistema</CardDescription>
        </CardHeader>
        <CardContent>
          {citas.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">
              No hay citas registradas. Agrega una para comenzar.
            </p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Cliente</TableHead>
                  <TableHead>Fecha</TableHead>
                  <TableHead>Descripción</TableHead>
                  <TableHead>Estado</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {citas.map((cita) => (
                  <TableRow key={cita.id}>
                    <TableCell className="font-medium">{getClienteNombre(cita.cliente_id)}</TableCell>
                    <TableCell>{new Date(cita.fecha).toLocaleString("es-ES")}</TableCell>
                    <TableCell>{cita.descripcion}</TableCell>
                    <TableCell>
                      <span
                        className={`inline-flex items-center rounded-full px-2 py-1 text-xs font-medium ${getEstadoBadge(cita.estado)}`}
                      >
                        {cita.estado.replace("_", " ")}
                      </span>
                    </TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(cita)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleDelete(cita.id)}>
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </div>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  )
}

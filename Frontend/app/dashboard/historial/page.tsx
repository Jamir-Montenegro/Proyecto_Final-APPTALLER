"use client"

import type React from "react"

import { useEffect, useState } from "react"
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
import { Plus, Pencil, Trash2 } from "lucide-react"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Textarea } from "@/components/ui/textarea"

type Historial = {
  id: string
  fecha: string
  descripcion: string
  costo: number
  mecanico: string
  notas: string
}

export default function HistorialPage() {
  const [historial, setHistorial] = useState<Historial[]>([
    {
      id: "1",
      fecha: "2025-01-10",
      descripcion: "Cambio de aceite completo",
      costo: 45.99,
      mecanico: "Pedro Martínez",
      notas: "Aceite sintético 5W-30",
    },
    {
      id: "2",
      fecha: "2025-01-08",
      descripcion: "Reparación de frenos delanteros",
      costo: 180.5,
      mecanico: "Ana Rodríguez",
      notas: "Pastillas y discos nuevos",
    },
    {
      id: "3",
      fecha: "2025-01-05",
      descripcion: "Diagnóstico electrónico",
      costo: 35.0,
      mecanico: "Luis Fernández",
      notas: "Revisión de sensores",
    },
  ])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingHistorial, setEditingHistorial] = useState<Historial | null>(null)
  const [formData, setFormData] = useState({
    fecha: new Date().toISOString().split("T")[0],
    descripcion: "",
    costo: 0,
    mecanico: "",
    notas: "",
    auto_id: "",
    cliente_id: "",
  })

  useEffect(() => {
    // fetchHistorial()
  }, [])

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (editingHistorial) {
      setHistorial(historial.map((h) => (h.id === editingHistorial.id ? { ...editingHistorial, ...formData } : h)))
    } else {
      setHistorial([...historial, { id: Date.now().toString(), ...formData }])
    }
    setIsDialogOpen(false)
    setEditingHistorial(null)
    setFormData({
      fecha: new Date().toISOString().split("T")[0],
      descripcion: "",
      costo: 0,
      mecanico: "",
      notas: "",
      auto_id: "",
      cliente_id: "",
    })
  }

  const handleEdit = (item: Historial) => {
    setEditingHistorial(item)
    setFormData({
      fecha: item.fecha,
      descripcion: item.descripcion,
      costo: item.costo,
      mecanico: item.mecanico,
      notas: item.notas,
      auto_id: "",
      cliente_id: "",
    })
    setIsDialogOpen(true)
  }

  const handleDelete = (id: string) => {
    setHistorial(historial.filter((h) => h.id !== id))
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Historial de Servicios</h1>
          <p className="text-muted-foreground">Registro de todos los servicios realizados</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingHistorial(null)
                setFormData({
                  fecha: new Date().toISOString().split("T")[0],
                  descripcion: "",
                  costo: 0,
                  mecanico: "",
                  notas: "",
                  auto_id: "",
                  cliente_id: "",
                })
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Registro
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingHistorial ? "Editar Registro" : "Agregar Nuevo Registro"}</DialogTitle>
              <DialogDescription>Completa la información del servicio realizado</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid gap-2">
                <Label htmlFor="fecha">Fecha</Label>
                <Input
                  id="fecha"
                  type="date"
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
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="costo">Costo</Label>
                <Input
                  id="costo"
                  type="number"
                  step="0.01"
                  value={formData.costo}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      costo: Number.parseFloat(e.target.value),
                    })
                  }
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="mecanico">Mecánico</Label>
                <Input
                  id="mecanico"
                  value={formData.mecanico}
                  onChange={(e) => setFormData({ ...formData, mecanico: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="notas">Notas</Label>
                <Textarea
                  id="notas"
                  value={formData.notas}
                  onChange={(e) => setFormData({ ...formData, notas: e.target.value })}
                />
              </div>
              <Button type="submit" className="w-full">
                {editingHistorial ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Historial de Servicios</CardTitle>
          <CardDescription>Todos los servicios realizados en el taller</CardDescription>
        </CardHeader>
        <CardContent>
          {historial.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">
              No hay registros en el historial. Agrega uno para comenzar.
            </p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Fecha</TableHead>
                  <TableHead>Descripción</TableHead>
                  <TableHead>Mecánico</TableHead>
                  <TableHead>Costo</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {historial.map((item) => (
                  <TableRow key={item.id}>
                    <TableCell className="font-medium">{new Date(item.fecha).toLocaleDateString("es-ES")}</TableCell>
                    <TableCell>{item.descripcion}</TableCell>
                    <TableCell>{item.mecanico}</TableCell>
                    <TableCell>${item.costo.toFixed(2)}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(item)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleDelete(item.id)}>
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

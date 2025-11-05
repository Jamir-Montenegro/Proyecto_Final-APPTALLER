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
import { Plus, Pencil, Trash2 } from "lucide-react"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Textarea } from "@/components/ui/textarea"

type Inventario = {
  id: string
  nombre: string
  descripcion: string
  cantidad: number
  precio_unitario: number
  categoria: string
  proveedor: string
}

export default function InventarioPage() {
  const [inventario, setInventario] = useState<Inventario[]>([
    {
      id: "1",
      nombre: "Aceite Motor 5W-30",
      descripcion: "Aceite sintético para motor",
      cantidad: 24,
      precio_unitario: 12.99,
      categoria: "Lubricantes",
      proveedor: "AutoParts Inc",
    },
    {
      id: "2",
      nombre: "Filtro de Aire",
      descripcion: "Filtro de aire universal",
      cantidad: 15,
      precio_unitario: 8.5,
      categoria: "Filtros",
      proveedor: "FilterPro",
    },
    {
      id: "3",
      nombre: "Pastillas de Freno",
      descripcion: "Pastillas cerámicas delanteras",
      cantidad: 8,
      precio_unitario: 45.0,
      categoria: "Frenos",
      proveedor: "BrakeMaster",
    },
  ])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingItem, setEditingItem] = useState<Inventario | null>(null)
  const [formData, setFormData] = useState({
    nombre: "",
    descripcion: "",
    cantidad: 0,
    precio_unitario: 0,
    categoria: "",
    proveedor: "",
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (editingItem) {
      setInventario(inventario.map((i) => (i.id === editingItem.id ? { ...editingItem, ...formData } : i)))
    } else {
      setInventario([...inventario, { id: Date.now().toString(), ...formData }])
    }
    setIsDialogOpen(false)
    setEditingItem(null)
    setFormData({
      nombre: "",
      descripcion: "",
      cantidad: 0,
      precio_unitario: 0,
      categoria: "",
      proveedor: "",
    })
  }

  const handleEdit = (item: Inventario) => {
    setEditingItem(item)
    setFormData({
      nombre: item.nombre,
      descripcion: item.descripcion,
      cantidad: item.cantidad,
      precio_unitario: item.precio_unitario,
      categoria: item.categoria,
      proveedor: item.proveedor,
    })
    setIsDialogOpen(true)
  }

  const handleDelete = (id: string) => {
    setInventario(inventario.filter((i) => i.id !== id))
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Inventario</h1>
          <p className="text-muted-foreground">Gestiona el inventario de repuestos y materiales</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingItem(null)
                setFormData({
                  nombre: "",
                  descripcion: "",
                  cantidad: 0,
                  precio_unitario: 0,
                  categoria: "",
                  proveedor: "",
                })
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Item
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingItem ? "Editar Item" : "Agregar Nuevo Item"}</DialogTitle>
              <DialogDescription>Completa la información del item de inventario</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid gap-2">
                <Label htmlFor="nombre">Nombre</Label>
                <Input
                  id="nombre"
                  value={formData.nombre}
                  onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
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
                <Label htmlFor="cantidad">Cantidad</Label>
                <Input
                  id="cantidad"
                  type="number"
                  value={formData.cantidad}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      cantidad: Number.parseInt(e.target.value),
                    })
                  }
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="precio_unitario">Precio Unitario</Label>
                <Input
                  id="precio_unitario"
                  type="number"
                  step="0.01"
                  value={formData.precio_unitario}
                  onChange={(e) =>
                    setFormData({
                      ...formData,
                      precio_unitario: Number.parseFloat(e.target.value),
                    })
                  }
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="categoria">Categoría</Label>
                <Input
                  id="categoria"
                  value={formData.categoria}
                  onChange={(e) => setFormData({ ...formData, categoria: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="proveedor">Proveedor</Label>
                <Input
                  id="proveedor"
                  value={formData.proveedor}
                  onChange={(e) => setFormData({ ...formData, proveedor: e.target.value })}
                />
              </div>
              <Button type="submit" className="w-full">
                {editingItem ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Inventario</CardTitle>
          <CardDescription>Todos los items en el inventario del taller</CardDescription>
        </CardHeader>
        <CardContent>
          {inventario.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">
              No hay items en el inventario. Agrega uno para comenzar.
            </p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Nombre</TableHead>
                  <TableHead>Categoría</TableHead>
                  <TableHead>Cantidad</TableHead>
                  <TableHead>Precio</TableHead>
                  <TableHead>Proveedor</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {inventario.map((item) => (
                  <TableRow key={item.id}>
                    <TableCell className="font-medium">{item.nombre}</TableCell>
                    <TableCell>{item.categoria}</TableCell>
                    <TableCell>{item.cantidad}</TableCell>
                    <TableCell>${item.precio_unitario.toFixed(2)}</TableCell>
                    <TableCell>{item.proveedor}</TableCell>
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

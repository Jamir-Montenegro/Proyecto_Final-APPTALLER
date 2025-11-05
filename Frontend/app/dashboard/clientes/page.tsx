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

type Cliente = {
  id: string
  nombre: string
  telefono: string
  email: string
  direccion: string
}

export default function ClientesPage() {
  const [clientes, setClientes] = useState<Cliente[]>([
    {
      id: "1",
      nombre: "Juan Pérez",
      telefono: "555-0101",
      email: "juan@example.com",
      direccion: "Calle Principal 123",
    },
    {
      id: "2",
      nombre: "María García",
      telefono: "555-0102",
      email: "maria@example.com",
      direccion: "Avenida Central 456",
    },
    {
      id: "3",
      nombre: "Carlos López",
      telefono: "555-0103",
      email: "carlos@example.com",
      direccion: "Boulevard Norte 789",
    },
  ])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingCliente, setEditingCliente] = useState<Cliente | null>(null)
  const [formData, setFormData] = useState({
    nombre: "",
    telefono: "",
    email: "",
    direccion: "",
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (editingCliente) {
      setClientes(clientes.map((c) => (c.id === editingCliente.id ? { ...editingCliente, ...formData } : c)))
    } else {
      setClientes([...clientes, { id: Date.now().toString(), ...formData }])
    }
    setIsDialogOpen(false)
    setEditingCliente(null)
    setFormData({
      nombre: "",
      telefono: "",
      email: "",
      direccion: "",
    })
  }

  const handleEdit = (cliente: Cliente) => {
    setEditingCliente(cliente)
    setFormData({
      nombre: cliente.nombre,
      telefono: cliente.telefono,
      email: cliente.email,
      direccion: cliente.direccion,
    })
    setIsDialogOpen(true)
  }

  const handleDelete = (id: string) => {
    setClientes(clientes.filter((c) => c.id !== id))
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Clientes</h1>
          <p className="text-muted-foreground">Gestiona la información de tus clientes</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingCliente(null)
                setFormData({
                  nombre: "",
                  telefono: "",
                  email: "",
                  direccion: "",
                })
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Cliente
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingCliente ? "Editar Cliente" : "Agregar Nuevo Cliente"}</DialogTitle>
              <DialogDescription>Completa la información del cliente</DialogDescription>
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
                <Label htmlFor="telefono">Teléfono</Label>
                <Input
                  id="telefono"
                  value={formData.telefono}
                  onChange={(e) => setFormData({ ...formData, telefono: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="email">Email</Label>
                <Input
                  id="email"
                  type="email"
                  value={formData.email}
                  onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="direccion">Dirección</Label>
                <Input
                  id="direccion"
                  value={formData.direccion}
                  onChange={(e) => setFormData({ ...formData, direccion: e.target.value })}
                />
              </div>
              <Button type="submit" className="w-full">
                {editingCliente ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Clientes</CardTitle>
          <CardDescription>Todos los clientes registrados en el sistema</CardDescription>
        </CardHeader>
        <CardContent>
          {clientes.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">
              No hay clientes registrados. Agrega uno para comenzar.
            </p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Nombre</TableHead>
                  <TableHead>Teléfono</TableHead>
                  <TableHead>Email</TableHead>
                  <TableHead>Dirección</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>
              <TableBody>
                {clientes.map((cliente) => (
                  <TableRow key={cliente.id}>
                    <TableCell className="font-medium">{cliente.nombre}</TableCell>
                    <TableCell>{cliente.telefono}</TableCell>
                    <TableCell>{cliente.email}</TableCell>
                    <TableCell>{cliente.direccion}</TableCell>
                    <TableCell className="text-right">
                      <div className="flex justify-end gap-2">
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(cliente)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => handleDelete(cliente.id)}>
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

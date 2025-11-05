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
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Plus, Pencil, Trash2 } from "lucide-react"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"

type Auto = {
  id: string
  marca: string
  modelo: string
  año: number
  placa: string
  color: string
  vin: string
  clienteId: string
}

type Cliente = {
  id: string
  nombre: string
}

export default function AutosPage() {
  const clientes: Cliente[] = [
    { id: "1", nombre: "Juan Pérez" },
    { id: "2", nombre: "María García" },
    { id: "3", nombre: "Carlos López" },
  ]

  const [autos, setAutos] = useState<Auto[]>([
    {
      id: "1",
      marca: "Toyota",
      modelo: "Corolla",
      año: 2020,
      placa: "ABC-123",
      color: "Blanco",
      vin: "1HGBH41JXMN109186",
      clienteId: "1",
    },
    {
      id: "2",
      marca: "Honda",
      modelo: "Civic",
      año: 2019,
      placa: "XYZ-789",
      color: "Negro",
      vin: "2HGFC2F59HH123456",
      clienteId: "2",
    },
    {
      id: "3",
      marca: "Ford",
      modelo: "Focus",
      año: 2021,
      placa: "DEF-456",
      color: "Azul",
      vin: "1FADP3K29FL123456",
      clienteId: "3",
    },
  ])
  const [isDialogOpen, setIsDialogOpen] = useState(false)
  const [editingAuto, setEditingAuto] = useState<Auto | null>(null)
  const [formData, setFormData] = useState({
    marca: "",
    modelo: "",
    año: new Date().getFullYear(),
    placa: "",
    color: "",
    vin: "",
    clienteId: "",
  })

  const getClienteName = (clienteId: string) => {
    const cliente = clientes.find((c) => c.id === clienteId)
    return cliente ? cliente.nombre : "Sin asignar"
  }

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    if (editingAuto) {
      setAutos(autos.map((a) => (a.id === editingAuto.id ? { ...editingAuto, ...formData } : a)))
    } else {
      setAutos([...autos, { id: Date.now().toString(), ...formData }])
    }
    setIsDialogOpen(false)
    setEditingAuto(null)
    setFormData({
      marca: "",
      modelo: "",
      año: new Date().getFullYear(),
      placa: "",
      color: "",
      vin: "",
      clienteId: "",
    })
  }

  const handleEdit = (auto: Auto) => {
    setEditingAuto(auto)
    setFormData({
      marca: auto.marca,
      modelo: auto.modelo,
      año: auto.año,
      placa: auto.placa,
      color: auto.color,
      vin: auto.vin,
      clienteId: auto.clienteId,
    })
    setIsDialogOpen(true)
  }

  const handleDelete = (id: string) => {
    setAutos(autos.filter((a) => a.id !== id))
  }

  return (
    <div className="space-y-6">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Autos</h1>
          <p className="text-muted-foreground">Gestiona los vehículos del taller</p>
        </div>
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingAuto(null)
                setFormData({
                  marca: "",
                  modelo: "",
                  año: new Date().getFullYear(),
                  placa: "",
                  color: "",
                  vin: "",
                  clienteId: "",
                })
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Auto
            </Button>
          </DialogTrigger>
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingAuto ? "Editar Auto" : "Agregar Nuevo Auto"}</DialogTitle>
              <DialogDescription>Completa la información del vehículo</DialogDescription>
            </DialogHeader>
            <form onSubmit={handleSubmit} className="space-y-4">
              <div className="grid gap-2">
                <Label htmlFor="cliente">Cliente (Propietario)</Label>
                <Select
                  value={formData.clienteId}
                  onValueChange={(value) => setFormData({ ...formData, clienteId: value })}
                  required
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecciona un cliente" />
                  </SelectTrigger>
                  <SelectContent>
                    {clientes.map((cliente) => (
                      <SelectItem key={cliente.id} value={cliente.id}>
                        {cliente.nombre}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
              <div className="grid gap-2">
                <Label htmlFor="marca">Marca</Label>
                <Input
                  id="marca"
                  value={formData.marca}
                  onChange={(e) => setFormData({ ...formData, marca: e.target.value })}
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="modelo">Modelo</Label>
                <Input
                  id="modelo"
                  value={formData.modelo}
                  onChange={(e) => setFormData({ ...formData, modelo: e.target.value })}
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="año">Año</Label>
                <Input
                  id="año"
                  type="number"
                  value={formData.año}
                  onChange={(e) => setFormData({ ...formData, año: Number.parseInt(e.target.value) })}
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="placa">Placa</Label>
                <Input
                  id="placa"
                  value={formData.placa}
                  onChange={(e) => setFormData({ ...formData, placa: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="color">Color</Label>
                <Input
                  id="color"
                  value={formData.color}
                  onChange={(e) => setFormData({ ...formData, color: e.target.value })}
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="vin">VIN</Label>
                <Input
                  id="vin"
                  value={formData.vin}
                  onChange={(e) => setFormData({ ...formData, vin: e.target.value })}
                />
              </div>
              <Button type="submit" className="w-full">
                {editingAuto ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Autos</CardTitle>
          <CardDescription>Todos los vehículos registrados en el sistema</CardDescription>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Cliente</TableHead>
                <TableHead>Marca</TableHead>
                <TableHead>Modelo</TableHead>
                <TableHead>Año</TableHead>
                <TableHead>Placa</TableHead>
                <TableHead>Color</TableHead>
                <TableHead className="text-right">Acciones</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {autos.map((auto) => (
                <TableRow key={auto.id}>
                  <TableCell className="font-medium">{getClienteName(auto.clienteId)}</TableCell>
                  <TableCell className="font-medium">{auto.marca}</TableCell>
                  <TableCell>{auto.modelo}</TableCell>
                  <TableCell>{auto.año}</TableCell>
                  <TableCell>{auto.placa}</TableCell>
                  <TableCell>{auto.color}</TableCell>
                  <TableCell className="text-right">
                    <div className="flex justify-end gap-2">
                      <Button variant="ghost" size="icon" onClick={() => handleEdit(auto)}>
                        <Pencil className="h-4 w-4" />
                      </Button>
                      <Button variant="ghost" size="icon" onClick={() => handleDelete(auto.id)}>
                        <Trash2 className="h-4 w-4" />
                      </Button>
                    </div>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </CardContent>
      </Card>
    </div>
  )
}

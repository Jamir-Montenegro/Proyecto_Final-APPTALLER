"use client";

import { useEffect, useState } from "react";
import { Plus, Pencil, Trash2 } from "lucide-react";

import { Button } from "@/components/ui/button";
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Select, SelectTrigger, SelectValue, SelectContent, SelectItem } from "@/components/ui/select";

import { useVehiculos } from "@/hooks/use-vehiculos";
import { useClientes } from "@/hooks/use-clientes";

export default function AutosPage() {
  const {
    vehiculos,
    cargarVehiculos,
    crearVehiculo,
    actualizarVehiculo,
    eliminarVehiculo,
  } = useVehiculos();

  const { clientes, fetchClientes } = useClientes();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingId, setEditingId] = useState<string | null>(null);

  const [formData, setFormData] = useState({
    clienteId: "",
    marca: "",
    modelo: "",
    anio: new Date().getFullYear(),
    placa: "",
    color: "",
    vin: "",
  });

  // Cargar datos al iniciar
  useEffect(() => {
    fetchClientes();
    cargarVehiculos();
  }, []);

  const resetForm = () => {
    setFormData({
      clienteId: "",
      marca: "",
      modelo: "",
      anio: new Date().getFullYear(),
      placa: "",
      color: "",
      vin: "",
    });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (editingId) {
      await actualizarVehiculo(editingId, formData);
    } else {
      await crearVehiculo(formData);
    }

    setIsDialogOpen(false);
    setEditingId(null);
    resetForm();
  };

  const handleEdit = (v: any) => {
    setEditingId(v.id);
    setFormData({
      clienteId: v.clienteId,
      marca: v.marca,
      modelo: v.modelo,
      anio: v.anio,
      placa: v.placa,
      color: v.color,
      vin: v.vin,
    });
    setIsDialogOpen(true);
  };

  const getClienteName = (id: string) => {
    const c = clientes.find((x) => x.id === id);
    return c ? `${c.nombre} (${c.cedula})` : "Sin asignar";
  };

  return (
    <div className="space-y-6">
      {/* HEADER */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Autos</h1>
          <p className="text-muted-foreground">Gestiona los vehículos del taller</p>
        </div>

        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                resetForm();
                setEditingId(null);
              }}
            >
              <Plus className="mr-2 h-4 w-4" /> Agregar Auto
            </Button>
          </DialogTrigger>

          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingId ? "Editar Auto" : "Agregar Nuevo Auto"}</DialogTitle>
              <DialogDescription>Completa los datos del vehículo</DialogDescription>
            </DialogHeader>

            {/* FORM */}
            <form onSubmit={handleSubmit} className="space-y-4">

              <div className="grid gap-2">
                <Label>Cliente</Label>
                <Select
                  value={formData.clienteId}
                  onValueChange={(v) => setFormData({ ...formData, clienteId: v })}
                >
                  <SelectTrigger>
                    <SelectValue placeholder="Selecciona un cliente" />
                  </SelectTrigger>
                  <SelectContent>
                    {clientes.map((c) => (
                      <SelectItem key={c.id} value={c.id}>
                        {c.nombre} ({c.cedula})
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="grid gap-2">
                <Label>Marca</Label>
                <Input
                  value={formData.marca}
                  onChange={(e) => setFormData({ ...formData, marca: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Modelo</Label>
                <Input
                  value={formData.modelo}
                  onChange={(e) => setFormData({ ...formData, modelo: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Año</Label>
                <Input
                  type="number"
                  value={formData.anio}
                  onChange={(e) =>
                    setFormData({ ...formData, anio: Number(e.target.value) })
                  }
                />
              </div>

              <div className="grid gap-2">
                <Label>Placa</Label>
                <Input
                  value={formData.placa}
                  onChange={(e) => setFormData({ ...formData, placa: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Color</Label>
                <Input
                  value={formData.color}
                  onChange={(e) => setFormData({ ...formData, color: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>VIN</Label>
                <Input
                  value={formData.vin}
                  onChange={(e) => setFormData({ ...formData, vin: e.target.value })}
                />
              </div>

              <Button type="submit" className="w-full">
                {editingId ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* TABLE */}
      <Card>
        <CardHeader>
          <CardTitle>Lista de Autos</CardTitle>
          <CardDescription>Vehículos registrados en el sistema</CardDescription>
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
              {vehiculos.map((v) => (
                <TableRow key={v.id}>
                  <TableCell>{getClienteName(v.clienteId)}</TableCell>
                  <TableCell>{v.marca}</TableCell>
                  <TableCell>{v.modelo}</TableCell>
                  <TableCell>{v.anio}</TableCell>
                  <TableCell>{v.placa}</TableCell>
                  <TableCell>{v.color}</TableCell>

                  <TableCell className="text-right">
                    <div className="flex justify-end gap-2">
                      <Button variant="ghost" size="icon" onClick={() => handleEdit(v)}>
                        <Pencil className="h-4 w-4" />
                      </Button>

                      <Button
                        variant="ghost"
                        size="icon"
                        onClick={() => eliminarVehiculo(v.id)}
                      >
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
  );
}

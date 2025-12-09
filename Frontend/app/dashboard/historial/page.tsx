"use client";

import type React from "react";
import { useEffect, useState } from "react";

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
import { Plus, Pencil, Trash2 } from "lucide-react";
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table";
import { Textarea } from "@/components/ui/textarea";

import { useVehiculos } from "@/hooks/use-vehiculos";
import { useServicios } from "@/hooks/use-servicios";

export default function HistorialPage() {
  const { vehiculos, cargarVehiculos } = useVehiculos();
  const { servicios, loadServicios, createServicio, updateServicio, deleteServicio } = useServicios();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingServicio, setEditingServicio] = useState<any>(null);

  const [formData, setFormData] = useState({
    fecha: new Date().toISOString().split("T")[0],
    descripcion: "",
    costo: 0,
    mecanico: "",
    notas: "",
    vehiculoId: "",
  });

  useEffect(() => {
    loadServicios();
    cargarVehiculos();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const payload = {
      fecha: formData.fecha,
      descripcion: formData.descripcion,
      costo: formData.costo,
      mecanico: formData.mecanico,
      notas: formData.notas,
      vehiculoId: formData.vehiculoId,
    };

    if (editingServicio) {
      await updateServicio(editingServicio.id, payload);
    } else {
      await createServicio(payload);
    }

    setIsDialogOpen(false);
    setEditingServicio(null);
    setFormData({
      fecha: new Date().toISOString().split("T")[0],
      descripcion: "",
      costo: 0,
      mecanico: "",
      notas: "",
      vehiculoId: "",
    });
  };

  const handleEdit = (s: any) => {
    setEditingServicio(s);
    setFormData({
      fecha: s.fecha,
      descripcion: s.descripcion,
      costo: s.costo,
      mecanico: s.mecanico,
      notas: s.notas,
      vehiculoId: s.vehiculoId,
    });
    setIsDialogOpen(true);
  };

  return (
    <div className="space-y-6">
      {/* ENCABEZADO */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Historial de Servicios</h1>
          <p className="text-muted-foreground">Registro de todos los servicios realizados</p>
        </div>

        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingServicio(null);
                setFormData({
                  fecha: new Date().toISOString().split("T")[0],
                  descripcion: "",
                  costo: 0,
                  mecanico: "",
                  notas: "",
                  vehiculoId: "",
                });
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Registro
            </Button>
          </DialogTrigger>

          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingServicio ? "Editar Registro" : "Agregar Nuevo Registro"}</DialogTitle>
              <DialogDescription>Completa la información del servicio realizado</DialogDescription>
            </DialogHeader>

            <form onSubmit={handleSubmit} className="space-y-4">
              {/* FECHA */}
              <div className="grid gap-2">
                <Label>Fecha</Label>
                <Input
                  type="date"
                  value={formData.fecha}
                  onChange={(e) => setFormData({ ...formData, fecha: e.target.value })}
                />
              </div>

              {/* VEHÍCULO (Dropdown con placa + cliente) */}
              <div className="grid gap-2">
                <Label>Vehículo</Label>
                <select
                  className="h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  value={formData.vehiculoId}
                  onChange={(e) => setFormData({ ...formData, vehiculoId: e.target.value })}
                  required
                >
                  <option value="">Selecciona un vehículo</option>

                  {vehiculos.map((v) => (
                    <option key={v.id} value={v.id}>
                      {v.placa} — {v.marca} {v.modelo} — {v.clienteNombre}
                    </option>
                  ))}
                </select>
              </div>

              {/* DESCRIPCIÓN */}
              <div className="grid gap-2">
                <Label>Descripción</Label>
                <Textarea
                  value={formData.descripcion}
                  onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })}
                />
              </div>

              {/* COSTO */}
              <div className="grid gap-2">
                <Label>Costo</Label>
                <Input
                  type="number"
                  step="0.01"
                  value={formData.costo}
                  onChange={(e) => setFormData({ ...formData, costo: parseFloat(e.target.value) })}
                />
              </div>

              {/* MECÁNICO */}
              <div className="grid gap-2">
                <Label>Mecánico</Label>
                <Input
                  value={formData.mecanico}
                  onChange={(e) => setFormData({ ...formData, mecanico: e.target.value })}
                />
              </div>

              {/* NOTAS */}
              <div className="grid gap-2">
                <Label>Notas</Label>
                <Textarea
                  value={formData.notas}
                  onChange={(e) => setFormData({ ...formData, notas: e.target.value })}
                />
              </div>

              <Button type="submit" className="w-full">
                {editingServicio ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* TABLA */}
      <Card>
        <CardHeader>
          <CardTitle>Historial de Servicios</CardTitle>
          <CardDescription>Todos los servicios realizados en el taller</CardDescription>
        </CardHeader>

        <CardContent>
          {servicios.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">No hay registros en el historial.</p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Fecha</TableHead>
                  <TableHead>Vehículo</TableHead>
                  <TableHead>Descripción</TableHead>
                  <TableHead>Mecánico</TableHead>
                  <TableHead>Costo</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>

              <TableBody>
                {servicios.map((s) => {
                  const v = vehiculos.find((x) => x.id === s.vehiculoId);

                  return (
                    <TableRow key={s.id}>
                      <TableCell>{new Date(s.fecha).toLocaleDateString("es-PA")}</TableCell>

                      <TableCell>
                        {v
                          ? `${v.placa} — ${v.marca} ${v.modelo} — ${v.clienteNombre}`
                          : "Vehículo no encontrado"}
                      </TableCell>

                      <TableCell>{s.descripcion}</TableCell>
                      <TableCell>{s.mecanico}</TableCell>
                      <TableCell>${s.costo.toFixed(2)}</TableCell>

                      <TableCell className="text-right">
                        <Button variant="ghost" size="icon" onClick={() => handleEdit(s)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button variant="ghost" size="icon" onClick={() => deleteServicio(s.id)}>
                          <Trash2 className="h-4 w-4" />
                        </Button>
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          )}
        </CardContent>
      </Card>
    </div>
  );
}



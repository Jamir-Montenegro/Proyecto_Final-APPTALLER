"use client";

import type React from "react";
import { useState } from "react";

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

import { useMateriales } from "@/hooks/use-materiales";

export default function InventarioPage() {
  const { materiales, createMaterial, updateMaterial, deleteMaterial } = useMateriales();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingItem, setEditingItem] = useState<any>(null);

  const [formData, setFormData] = useState({
    nombre: "",
    descripcion: "",
    cantidad: 0,
    precioUnitario: 0,
    categoria: "",
    proveedor: "",
    umbralBajo: 0, // ⭐ NECESARIO PARA EVITAR ERROR DE TYPESCRIPT
  });

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (editingItem) {
      await updateMaterial(editingItem.id, formData);
    } else {
      await createMaterial(formData);
    }

    setIsDialogOpen(false);
    setEditingItem(null);

    setFormData({
      nombre: "",
      descripcion: "",
      cantidad: 0,
      precioUnitario: 0,
      categoria: "",
      proveedor: "",
      umbralBajo: 0,
    });
  };

  const handleEdit = (item: any) => {
    setEditingItem(item);
    setFormData({
      nombre: item.nombre,
      descripcion: item.descripcion,
      cantidad: item.cantidad,
      precioUnitario: item.precioUnitario,
      categoria: item.categoria,
      proveedor: item.proveedor,
      umbralBajo: item.umbralBajo ?? 0, // ⭐ SI NO EXISTE, LE PONE 0
    });

    setIsDialogOpen(true);
  };

  return (
    <div className="space-y-6">

      {/* HEADER */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Inventario</h1>
          <p className="text-muted-foreground">Gestiona los materiales y repuestos del taller</p>
        </div>

        {/* BUTTON */}
        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingItem(null);
                setFormData({
                  nombre: "",
                  descripcion: "",
                  cantidad: 0,
                  precioUnitario: 0,
                  categoria: "",
                  proveedor: "",
                  umbralBajo: 0,
                });
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Material
            </Button>
          </DialogTrigger>

          {/* MODAL */}
          <DialogContent>
            <DialogHeader>
              <DialogTitle>{editingItem ? "Editar Material" : "Agregar Nuevo Material"}</DialogTitle>
              <DialogDescription>Completa la información del material</DialogDescription>
            </DialogHeader>

            {/* FORMULARIO */}
            <form onSubmit={handleSubmit} className="space-y-4">
              
              <div className="grid gap-2">
                <Label>Nombre</Label>
                <Input
                  value={formData.nombre}
                  onChange={(e) => setFormData({ ...formData, nombre: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Descripción</Label>
                <Textarea
                  value={formData.descripcion}
                  onChange={(e) => setFormData({ ...formData, descripcion: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Cantidad</Label>
                <Input
                  type="number"
                  value={formData.cantidad}
                  onChange={(e) => setFormData({ ...formData, cantidad: Number(e.target.value) })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Precio Unitario</Label>
                <Input
                  type="number"
                  step="0.01"
                  value={formData.precioUnitario}
                  onChange={(e) =>
                    setFormData({ ...formData, precioUnitario: Number(e.target.value) })
                  }
                />
              </div>

              <div className="grid gap-2">
                <Label>Categoría</Label>
                <Input
                  value={formData.categoria}
                  onChange={(e) => setFormData({ ...formData, categoria: e.target.value })}
                />
              </div>

              <div className="grid gap-2">
                <Label>Proveedor</Label>
                <Input
                  value={formData.proveedor}
                  onChange={(e) => setFormData({ ...formData, proveedor: e.target.value })}
                />
              </div>

              {/* ⭐ UMBRAL BAJO */}
              <div className="grid gap-2">
                <Label>Umbral Bajo</Label>
                <Input
                  type="number"
                  value={formData.umbralBajo}
                  onChange={(e) =>
                    setFormData({ ...formData, umbralBajo: Number(e.target.value) })
                  }
                />
              </div>

              <Button type="submit" className="w-full">
                {editingItem ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* TABLA */}
      <Card>
        <CardHeader>
          <CardTitle>Lista de Inventario</CardTitle>
          <CardDescription>Todos los materiales registrados</CardDescription>
        </CardHeader>

        <CardContent>
          {materiales.length === 0 ? (
            <p className="text-center text-muted-foreground py-8">No hay materiales registrados.</p>
          ) : (
            <Table>
              <TableHeader>
                <TableRow>
                  <TableHead>Nombre</TableHead>
                  <TableHead>Categoría</TableHead>
                  <TableHead>Cantidad</TableHead>
                  <TableHead>Umbral</TableHead>
                  <TableHead>Precio</TableHead>
                  <TableHead>Proveedor</TableHead>
                  <TableHead className="text-right">Acciones</TableHead>
                </TableRow>
              </TableHeader>

              <TableBody>
                {materiales.map((item) => {
                  const lowStock = item.cantidad <= item.umbralBajo;

                  return (
                    <TableRow key={item.id} className={lowStock ? "bg-red-500/10" : ""}>
                      <TableCell>{item.nombre}</TableCell>
                      <TableCell>{item.categoria}</TableCell>

                      <TableCell className={lowStock ? "text-red-600 font-bold" : ""}>
                        {item.cantidad}
                      </TableCell>

                      <TableCell>{item.umbralBajo}</TableCell>

                      <TableCell>${item.precioUnitario.toFixed(2)}</TableCell>

                      <TableCell>{item.proveedor}</TableCell>

                      <TableCell className="text-right">
                        <div className="flex justify-end gap-2">
                          <Button variant="ghost" size="icon" onClick={() => handleEdit(item)}>
                            <Pencil className="h-4 w-4" />
                          </Button>

                          <Button variant="ghost" size="icon" onClick={() => deleteMaterial(item.id)}>
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        </div>
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

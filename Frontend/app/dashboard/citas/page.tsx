"use client";

import { useState, useEffect } from "react";

import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
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
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Textarea } from "@/components/ui/textarea";

// Hooks reales del backend
import { useClientes } from "@/hooks/use-clientes";
import { useCitas } from "@/hooks/use-citas";

export default function CitasPage() {
  const { clientes, fetchClientes } = useClientes();

  useEffect(() => {
    fetchClientes();
    loadCitas();
  }, []);

  const { citas, createCita, updateCita, deleteCita, loadCitas } = useCitas();

  const [isDialogOpen, setIsDialogOpen] = useState(false);
  const [editingCita, setEditingCita] = useState<any>(null);

  const [formData, setFormData] = useState({
    clienteId: "",
    fechaHora: "",
    descripcion: "",
    estado: "Pendiente",
  });

  // Cargar clientes y citas al entrar
  useEffect(() => {
    fetchClientes();
    loadCitas();
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const isoDate = new Date(formData.fechaHora).toISOString();

    const payload = {
      clienteId: formData.clienteId,
      fechaHora: isoDate,
      descripcion: formData.descripcion,
      estado: formData.estado,
    };

    if (editingCita) {
      await updateCita(editingCita.id, payload);
    } else {
      await createCita(payload);
    }

    setIsDialogOpen(false);
    setEditingCita(null);
    setFormData({
      clienteId: "",
      fechaHora: "",
      descripcion: "",
      estado: "Pendiente",
    });
  };

  const handleEdit = (cita: any) => {
    setEditingCita(cita);
    setFormData({
      clienteId: cita.clienteId,
      fechaHora: cita.fechaHora.slice(0, 16),
      descripcion: cita.descripcion ?? "",
      estado: cita.estado,
    });
    setIsDialogOpen(true);
  };

  const getEstadoBadge = (estado: string) => {
    const colors: Record<
      "Pendiente" | "En Progreso" | "Completada" | "Cancelada",
      string
    > = {
      Pendiente: "bg-yellow-500/10 text-yellow-500",
      "En Progreso": "bg-blue-500/10 text-blue-500",
      Completada: "bg-green-500/10 text-green-500",
      Cancelada: "bg-red-500/10 text-red-500",
    };

    return colors[estado as keyof typeof colors] ?? colors["Pendiente"];
  };

  return (
    <div className="space-y-6">
      {/* ENCABEZADO */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Citas</h1>
          <p className="text-muted-foreground">Gestiona las citas del taller</p>
        </div>

        <Dialog open={isDialogOpen} onOpenChange={setIsDialogOpen}>
          <DialogTrigger asChild>
            <Button
              onClick={() => {
                setEditingCita(null);
                setFormData({
                  clienteId: "",
                  fechaHora: "",
                  descripcion: "",
                  estado: "Pendiente",
                });
              }}
            >
              <Plus className="mr-2 h-4 w-4" />
              Agregar Cita
            </Button>
          </DialogTrigger>

          <DialogContent>
            <DialogHeader>
              <DialogTitle>
                {editingCita ? "Editar Cita" : "Agregar Nueva Cita"}
              </DialogTitle>
              <DialogDescription>Completa la información</DialogDescription>
            </DialogHeader>

            <form onSubmit={handleSubmit} className="space-y-4">
              {/* CLIENTE */}
              <div className="grid gap-2">
                <Label htmlFor="cliente">Cliente</Label>
                <select
                  id="cliente"
                  className="h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  value={formData.clienteId}
                  onChange={(e) =>
                    setFormData({ ...formData, clienteId: e.target.value })
                  }
                  required
                >
                  <option value="">Selecciona un cliente</option>

                  {clientes.map((c) => (
                    <option key={c.id} value={c.id}>
                      {c.nombre} ({c.cedula})
                    </option>
                  ))}
                </select>
              </div>

              {/* FECHA */}
              <div className="grid gap-2">
                <Label htmlFor="fechaHora">Fecha y Hora</Label>
                <Input
                  id="fechaHora"
                  type="datetime-local"
                  value={formData.fechaHora}
                  onChange={(e) =>
                    setFormData({ ...formData, fechaHora: e.target.value })
                  }
                  required
                />
              </div>

              {/* DESCRIPCIÓN */}
              <div className="grid gap-2">
                <Label htmlFor="descripcion">Descripción</Label>
                <Textarea
                  id="descripcion"
                  value={formData.descripcion}
                  onChange={(e) =>
                    setFormData({ ...formData, descripcion: e.target.value })
                  }
                />
              </div>

              {/* ESTADO */}
              <div className="grid gap-2">
                <Label htmlFor="estado">Estado</Label>
                <select
                  id="estado"
                  className="h-10 w-full rounded-md border border-input bg-background px-3 py-2 text-sm"
                  value={formData.estado}
                  onChange={(e) =>
                    setFormData({ ...formData, estado: e.target.value })
                  }
                >
                  <option value="Pendiente">Pendiente</option>
                  <option value="En Progreso">En Progreso</option>
                  <option value="Completada">Completada</option>
                  <option value="Cancelada">Cancelada</option>
                </select>
              </div>

              <Button type="submit" className="w-full">
                {editingCita ? "Actualizar" : "Agregar"}
              </Button>
            </form>
          </DialogContent>
        </Dialog>
      </div>

      {/* TABLA */}
      <Card>
        <CardHeader>
          <CardTitle>Lista de Citas</CardTitle>
          <CardDescription>Todas las citas registradas</CardDescription>
        </CardHeader>

        <CardContent>
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
                  <TableCell>
                    {(() => {
                      const c = clientes.find((x) => x.id === cita.clienteId);
                      return c ? `${c.nombre} (${c.cedula})` : "Sin cliente";
                    })()}
                  </TableCell>

                  <TableCell>
                    {new Date(cita.fechaHora).toLocaleString("es-PA")}
                  </TableCell>

                  <TableCell>{cita.descripcion || "—"}</TableCell>

                  <TableCell>
                    <span
                      className={`inline-flex items-center rounded-full px-2 py-1 text-xs font-medium ${getEstadoBadge(
                        cita.estado
                      )}`}
                    >
                      {cita.estado}
                    </span>
                  </TableCell>

                  <TableCell className="text-right">
                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => handleEdit(cita)}
                    >
                      <Pencil className="h-4 w-4" />
                    </Button>

                    <Button
                      variant="ghost"
                      size="icon"
                      onClick={() => deleteCita(cita.id)}
                    >
                      <Trash2 className="h-4 w-4" />
                    </Button>
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

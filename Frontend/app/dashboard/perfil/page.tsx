"use client";

import type React from "react";
import { useEffect, useState } from "react";
import { Avatar, AvatarFallback } from "@/components/ui/avatar";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

type PerfilData = {
  nombreTaller: string;
  email: string;
};

export default function PerfilPage() {
  const [perfil, setPerfil] = useState<PerfilData>({
    nombreTaller: "",
    email: "",
  });


  useEffect(() => {
    if (typeof window === "undefined") return;

    const nombre = localStorage.getItem("nombre") ?? "";
    const email = localStorage.getItem("email") ?? "";

    setPerfil({
      nombreTaller: nombre,
      email,
    });
  }, []);

  const inicial = (perfil.nombreTaller || perfil.email || "?")
    .charAt(0)
    .toUpperCase();

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold">Perfil</h1>
        <p className="text-muted-foreground">Información de tu cuenta</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Información del Taller</CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          {/* Avatar + encabezado */}
          <div className="flex items-center gap-4">
            <Avatar className="h-24 w-24">
              <AvatarFallback className="bg-blue-500 text-white text-3xl">
                {inicial}
              </AvatarFallback>
            </Avatar>

            <div>
              <h2 className="text-2xl font-semibold">
                {perfil.nombreTaller || "Taller sin nombre"}
              </h2>
              <p className="text-muted-foreground">
                {perfil.email || "Sin correo configurado"}
              </p>
            </div>
          </div>

          {/* Detalle */}
          <div className="space-y-6 pt-4">
            <div className="space-y-1">
              <p className="text-sm text-muted-foreground">Nombre del Taller</p>
              <p className="text-lg font-medium">
                {perfil.nombreTaller || "Taller sin nombre"}
              </p>
            </div>

            <div className="space-y-1">
              <p className="text-sm text-muted-foreground">
                Correo Electrónico
              </p>
              <p className="text-lg font-medium">
                {perfil.email || "Sin correo configurado"}
              </p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}

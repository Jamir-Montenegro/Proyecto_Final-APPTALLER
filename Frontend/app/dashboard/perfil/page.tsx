"use client"

import type React from "react"
import { Avatar, AvatarFallback } from "@/components/ui/avatar"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
// import { createClient } from "@/utils/supabaseClient" // Assuming supabaseClient is the correct import
import { useState } from "react"

export default function PerfilPage() {
  // const [user, setUser] = useState<any>(null)
  const [newPassword, setNewPassword] = useState("")
  const [confirmPassword, setConfirmPassword] = useState("")
  const [message, setMessage] = useState("")
  const [error, setError] = useState("")

  // const supabase = createClient()

  // useEffect(() => {
  //   fetchUser()
  // }, [])

  // const fetchUser = async () => {
  //   const {
  //     data: { user },
  //   } = await supabase.auth.getUser()
  //   if (user) {
  //     setUser(user)
  //     setEmail(user.email || "")
  //   }
  // }

  const handleUpdatePassword = (e: React.FormEvent) => {
    e.preventDefault()
    setMessage("")
    setError("")

    if (newPassword !== confirmPassword) {
      setError("Las contraseñas no coinciden")
      return
    }

    if (newPassword.length < 6) {
      setError("La contraseña debe tener al menos 6 caracteres")
      return
    }

    setMessage("Contraseña actualizada exitosamente")
    setNewPassword("")
    setConfirmPassword("")
  }

  const userName = "jamir"
  const userEmail = "jama@gmail.com"
  const workshopName = "aaaaaaaa"

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold">Perfil</h1>
        <p className="text-muted-foreground">Información de tu cuenta</p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Información Personal</CardTitle>
        </CardHeader>
        <CardContent className="space-y-6">
          <div className="flex items-center gap-4">
            <Avatar className="h-24 w-24">
              <AvatarFallback className="bg-blue-500 text-white text-3xl">
                {userName.charAt(0).toUpperCase()}
              </AvatarFallback>
            </Avatar>
            <div>
              <h2 className="text-2xl font-semibold">{userName}</h2>
              <p className="text-muted-foreground">{userEmail}</p>
            </div>
          </div>

          <div className="space-y-6 pt-4">
            <div className="space-y-2">
              <p className="text-sm text-muted-foreground">Nombre del Taller</p>
              <p className="text-lg font-medium">{workshopName}</p>
            </div>
            <div className="space-y-2">
              <p className="text-sm text-muted-foreground">Correo Electrónico</p>
              <p className="text-lg font-medium">{userEmail}</p>
            </div>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

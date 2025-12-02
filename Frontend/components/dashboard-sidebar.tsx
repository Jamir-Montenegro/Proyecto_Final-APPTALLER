"use client"

import Link from "next/link"
import { usePathname } from 'next/navigation'
import { cn } from "@/lib/utils"
import { Car, Users, Calendar, History, Package, User, LogOut, Menu, X, BarChart3, Moon, Sun } from 'lucide-react'
import { Button } from "@/components/ui/button"
import { useRouter } from 'next/navigation'
import { useState } from "react"
import { useTheme } from "@/components/theme-provider"

const navigation = [
  { name: "Autos", href: "/dashboard/autos", icon: Car },
  { name: "Clientes", href: "/dashboard/clientes", icon: Users },
  { name: "Citas", href: "/dashboard/citas", icon: Calendar },
  { name: "Historial", href: "/dashboard/historial", icon: History },
  { name: "Inventario", href: "/dashboard/inventario", icon: Package },
  { name: "Informe", href: "/dashboard/informe", icon: BarChart3 },
  { name: "Perfil", href: "/dashboard/perfil", icon: User },
]

export function DashboardSidebar() {
  const pathname = usePathname()
  const router = useRouter()
  const [mobileMenuOpen, setMobileMenuOpen] = useState(false)
  const { theme, toggleTheme } = useTheme()

  const handleLogout = async () => {
    router.push("/auth/login")
  }

  return (
    <>
      {/* Mobile menu button */}
      <div className="lg:hidden fixed top-4 left-4 z-50">
        <Button
          variant="outline"
          size="icon"
          onClick={() => setMobileMenuOpen(!mobileMenuOpen)}
          className="bg-white"
        >
          {mobileMenuOpen ? <X className="h-5 w-5" /> : <Menu className="h-5 w-5" />}
        </Button>
      </div>

      {/* Sidebar */}
      <aside
        className={cn(
          "fixed inset-y-0 left-0 z-40 w-64 bg-[#1E293B] border-r border-[#334155] transform transition-transform duration-200 ease-in-out lg:translate-x-0",
          mobileMenuOpen ? "translate-x-0" : "-translate-x-full",
        )}
      >
        <div className="flex h-full flex-col">
          {/* Logo */}
          <div className="flex h-16 items-center border-b border-[#334155] px-6">
            <h1 className="text-xl font-bold text-white">Taller de Chipstería</h1>
          </div>

          {/* Navigation */}
          <nav className="flex-1 space-y-1 px-3 py-4">
            {navigation.map((item) => {
              const isActive = pathname === item.href
              return (
                <Link
                  key={item.name}
                  href={item.href}
                  onClick={() => setMobileMenuOpen(false)}
                  className={cn(
                    "flex items-center gap-3 rounded-lg px-3 py-2 text-sm font-medium transition-colors",
                    isActive
                      ? "bg-[#DC2626] text-white"
                      : "text-gray-300 hover:bg-[#334155] hover:text-white",
                  )}
                >
                  <div
                    className={cn(
                      "p-1.5 rounded-md",
                      isActive ? "bg-white/20" : "bg-red-100"
                    )}
                  >
                    <item.icon className={cn("h-4 w-4", isActive ? "text-white" : "text-red-600")} />
                  </div>
                  {item.name}
                </Link>
              )
            })}
          </nav>

          {/* Theme toggle button */}
          <div className="border-t border-[#334155] p-4 space-y-2">
            <Button
              variant="outline"
              className="w-full justify-start gap-3 bg-transparent border-gray-600 text-gray-300 hover:bg-[#334155] hover:text-white"
              onClick={toggleTheme}
            >
              {theme === "light" ? (
                <>
                  <Moon className="h-5 w-5" />
                  Tema Oscuro
                </>
              ) : (
                <>
                  <Sun className="h-5 w-5" />
                  Tema Claro
                </>
              )}
            </Button>
            
            {/* Logout button */}
            <Button
              variant="outline"
              className="w-full justify-start gap-3 bg-transparent border-gray-600 text-gray-300 hover:bg-[#334155] hover:text-white"
              onClick={handleLogout}
            >
              <LogOut className="h-5 w-5" />
              Cerrar Sesión
            </Button>
          </div>
        </div>
      </aside>

      {/* Overlay for mobile */}
      {mobileMenuOpen && (
        <div
          className="fixed inset-0 z-30 bg-black/50 backdrop-blur-sm lg:hidden"
          onClick={() => setMobileMenuOpen(false)}
        />
      )}
    </>
  )
}

import type React from "react"
import { DashboardSidebar } from "@/components/dashboard-sidebar"
import { ThemeProvider } from "@/components/theme-provider"

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode
}) {
  return (
    <ThemeProvider>
      <div className="flex h-screen bg-background">
        <DashboardSidebar />
        <main className="flex-1 overflow-y-auto lg:ml-64">
          <div className="container mx-auto p-6 lg:p-8">{children}</div>
        </main>
      </div>
    </ThemeProvider>
  )
}

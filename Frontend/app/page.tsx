import { redirect } from "next/navigation"

export default function HomePage() {
  redirect("/auth/login")
}

export const metadata = {
  title: "Taller de Chipstería",
}

import { redirect } from "next/navigation"

export default function HomePage() {
  redirect("/dashboard")
}

export const metadata = {
  title: "Taller de Chipstería",
}

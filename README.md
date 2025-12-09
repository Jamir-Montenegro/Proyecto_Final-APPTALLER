# 🚗⚙️ AppTaller – Sistema de Gestión para Taller Automotriz  
![GitHub](https://img.shields.io/badge/Next.js-14-black?style=for-the-badge&logo=nextdotjs)
![GitHub](https://img.shields.io/badge/.NET-8-purple?style=for-the-badge&logo=dotnet)
![GitHub](https://img.shields.io/badge/PostgreSQL-Supabase-3c3c3c?style=for-the-badge&logo=postgresql)
![GitHub](https://img.shields.io/badge/PWA-Ready-teal?style=for-the-badge&logo=pwa)

AppTaller es un sistema completo para la administración de talleres automotrices.  
Permite gestionar clientes, vehículos, citas, inventario, historial de servicios e informes dentro de una interfaz moderna, rápida y responsiva.

El proyecto está dividido en **frontend (Next.js)** y **backend (.NET Web API + PostgreSQL)**, con autenticación por JWT y soporte completo para **PWA (instalable como app móvil o de escritorio)**.

---

# 📚 Tabla de Contenidos
- [✨ Características](#-características)
- [🛠️ Tecnologías](#️-tecnologías)
- [📦 Estructura del Proyecto](#-estructura-del-proyecto)
- [🌐 Frontend (Next.js)](#-frontend-nextjs)
- [🖥️ Backend (.NET 8 API)](#️-backend-net-8-api)
- [🗄️ Base de Datos (Supabase/PostgreSQL)](#️-base-de-datos-supabasepostgresql)
- [⚙️ Instalación](#️-instalación)
- [▶️ Ejecución](#️-ejecución)
- [📱 PWA – App Instal able](#-pwa--app-instalable)
- [📊 Funcionalidades Principales](#-funcionalidades-principales)
- [📌 Roadmap](#-roadmap)
- [👤 Autor](#-autor)
- [📄 Licencia](#-licencia)

---

# ✨ Características

### 🔐 Autenticación + Roles
- Registro e inicio de sesión de talleres.
- JWT + Claims con `tallerId` para aislar la información.

### 👥 Gestión de Clientes
- CRUD completo.
- Relación directa con vehículos.

### 🚗 Gestión de Vehículos
- Validación de placa única.
- Información completa: marca, modelo, año, color y VIN.
- Asignación a clientes.

### 📅 Gestión de Citas
- Estados: Pendiente, En Progreso, Completada, Cancelada.
- CRUD con modal UI moderno.

### 🧾 Historial de Servicios
- Registro detallado de trabajos realizados.
- Costo, mecánico, notas y fecha.

### 📦 Inventario de Materiales
- CRUD conectado a base de datos.
- Manejo de stock, proveedor, categoría y precio.
- Umbral bajo configurable.

### 📊 Dashboard e Informes
- Totales dinámicos: clientes, autos, citas.
- Tasa de cumplimiento.
- Promedio de vehículos por cliente.
- Citas por estado.

### 📱 PWA (Aplicación instalable)
- `manifest.json` configurado.
- Iconos multiplataforma.
- Service Worker listo.

---

# 🛠️ Tecnologías

### **Frontend**
- Next.js 14 (App Router)
- React 18
- TypeScript
- TailwindCSS
- Shadcn/UI
- Hooks personalizados
- PWA

### **Backend**
- .NET 8 Web API
- PostgreSQL (Supabase)
- JWT Authentication
- Arquitectura limpia (Controllers / Services / Repositories)

---



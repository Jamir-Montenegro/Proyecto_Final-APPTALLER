-- Enable UUID extension
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";

-- Clientes (Customers) table
CREATE TABLE IF NOT EXISTS clientes (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  nombre TEXT NOT NULL,
  telefono TEXT,
  email TEXT,
  direccion TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  user_id UUID REFERENCES auth.users(id) ON DELETE CASCADE
);

-- Autos (Cars) table
CREATE TABLE IF NOT EXISTS autos (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  cliente_id UUID REFERENCES clientes(id) ON DELETE CASCADE,
  marca TEXT NOT NULL,
  modelo TEXT NOT NULL,
  año INTEGER,
  placa TEXT,
  color TEXT,
  vin TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  user_id UUID REFERENCES auth.users(id) ON DELETE CASCADE
);

-- Citas (Appointments) table
CREATE TABLE IF NOT EXISTS citas (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  auto_id UUID REFERENCES autos(id) ON DELETE CASCADE,
  cliente_id UUID REFERENCES clientes(id) ON DELETE CASCADE,
  fecha TIMESTAMP WITH TIME ZONE NOT NULL,
  descripcion TEXT,
  estado TEXT DEFAULT 'pendiente' CHECK (estado IN ('pendiente', 'en_progreso', 'completada', 'cancelada')),
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  user_id UUID REFERENCES auth.users(id) ON DELETE CASCADE
);

-- Historial (Service History) table
CREATE TABLE IF NOT EXISTS historial (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  auto_id UUID REFERENCES autos(id) ON DELETE CASCADE,
  cliente_id UUID REFERENCES clientes(id) ON DELETE CASCADE,
  fecha TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  descripcion TEXT NOT NULL,
  costo DECIMAL(10, 2),
  mecanico TEXT,
  notas TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  user_id UUID REFERENCES auth.users(id) ON DELETE CASCADE
);

-- Inventario (Inventory) table
CREATE TABLE IF NOT EXISTS inventario (
  id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
  nombre TEXT NOT NULL,
  descripcion TEXT,
  cantidad INTEGER DEFAULT 0,
  precio_unitario DECIMAL(10, 2),
  categoria TEXT,
  proveedor TEXT,
  created_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  updated_at TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
  user_id UUID REFERENCES auth.users(id) ON DELETE CASCADE
);

-- Enable Row Level Security
ALTER TABLE clientes ENABLE ROW LEVEL SECURITY;
ALTER TABLE autos ENABLE ROW LEVEL SECURITY;
ALTER TABLE citas ENABLE ROW LEVEL SECURITY;
ALTER TABLE historial ENABLE ROW LEVEL SECURITY;
ALTER TABLE inventario ENABLE ROW LEVEL SECURITY;

-- RLS Policies for clientes
CREATE POLICY "Users can view their own clientes" ON clientes FOR SELECT USING (auth.uid() = user_id);
CREATE POLICY "Users can insert their own clientes" ON clientes FOR INSERT WITH CHECK (auth.uid() = user_id);
CREATE POLICY "Users can update their own clientes" ON clientes FOR UPDATE USING (auth.uid() = user_id);
CREATE POLICY "Users can delete their own clientes" ON clientes FOR DELETE USING (auth.uid() = user_id);

-- RLS Policies for autos
CREATE POLICY "Users can view their own autos" ON autos FOR SELECT USING (auth.uid() = user_id);
CREATE POLICY "Users can insert their own autos" ON autos FOR INSERT WITH CHECK (auth.uid() = user_id);
CREATE POLICY "Users can update their own autos" ON autos FOR UPDATE USING (auth.uid() = user_id);
CREATE POLICY "Users can delete their own autos" ON autos FOR DELETE USING (auth.uid() = user_id);

-- RLS Policies for citas
CREATE POLICY "Users can view their own citas" ON citas FOR SELECT USING (auth.uid() = user_id);
CREATE POLICY "Users can insert their own citas" ON citas FOR INSERT WITH CHECK (auth.uid() = user_id);
CREATE POLICY "Users can update their own citas" ON citas FOR UPDATE USING (auth.uid() = user_id);
CREATE POLICY "Users can delete their own citas" ON citas FOR DELETE USING (auth.uid() = user_id);

-- RLS Policies for historial
CREATE POLICY "Users can view their own historial" ON historial FOR SELECT USING (auth.uid() = user_id);
CREATE POLICY "Users can insert their own historial" ON historial FOR INSERT WITH CHECK (auth.uid() = user_id);
CREATE POLICY "Users can update their own historial" ON historial FOR UPDATE USING (auth.uid() = user_id);
CREATE POLICY "Users can delete their own historial" ON historial FOR DELETE USING (auth.uid() = user_id);

-- RLS Policies for inventario
CREATE POLICY "Users can view their own inventario" ON inventario FOR SELECT USING (auth.uid() = user_id);
CREATE POLICY "Users can insert their own inventario" ON inventario FOR INSERT WITH CHECK (auth.uid() = user_id);
CREATE POLICY "Users can update their own inventario" ON inventario FOR UPDATE USING (auth.uid() = user_id);
CREATE POLICY "Users can delete their own inventario" ON inventario FOR DELETE USING (auth.uid() = user_id);

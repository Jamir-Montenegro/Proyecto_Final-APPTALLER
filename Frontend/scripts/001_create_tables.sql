-- 1. Tabla: talleres (cada registro = un taller independiente)
CREATE TABLE talleres (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  nombre TEXT NOT NULL,
  email TEXT NOT NULL UNIQUE,
  password_hash TEXT NOT NULL,
  creado_en TIMESTAMPTZ DEFAULT NOW()
);

-- 2. Tabla: clientes
CREATE TABLE clientes (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  taller_id UUID NOT NULL REFERENCES talleres(id) ON DELETE CASCADE,
  nombre TEXT NOT NULL,
  telefono TEXT NOT NULL,
  email TEXT,
  direccion TEXT,
  creado_en TIMESTAMPTZ DEFAULT NOW()
);

-- 3. Tabla: vehiculos
CREATE TABLE vehiculos (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  taller_id UUID NOT NULL REFERENCES talleres(id) ON DELETE CASCADE,
  cliente_id UUID NOT NULL REFERENCES clientes(id) ON DELETE CASCADE,
  marca TEXT NOT NULL,
  modelo TEXT NOT NULL,
  anio INTEGER NOT NULL,
  placa TEXT NOT NULL,
  color TEXT,
  vin TEXT,
  creado_en TIMESTAMPTZ DEFAULT NOW(),
  UNIQUE (taller_id, placa)  -- La placa es única solo dentro del mismo taller
);

-- 4. Tabla: citas
CREATE TABLE citas (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  taller_id UUID NOT NULL REFERENCES talleres(id) ON DELETE CASCADE,
  cliente_id UUID NOT NULL REFERENCES clientes(id) ON DELETE CASCADE,
  fecha_hora TIMESTAMPTZ NOT NULL,
  descripcion TEXT,
  estado TEXT NOT NULL DEFAULT 'Pendiente',  -- Valores: 'Pendiente', 'En Progreso', 'Completada', 'Cancelada'
  creado_en TIMESTAMPTZ DEFAULT NOW()
);

-- 5. Tabla: servicios (historial de trabajos)
CREATE TABLE servicios (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  taller_id UUID NOT NULL REFERENCES talleres(id) ON DELETE CASCADE,
  vehiculo_id UUID NOT NULL REFERENCES vehiculos(id) ON DELETE CASCADE,
  fecha DATE NOT NULL,
  descripcion TEXT NOT NULL,
  costo NUMERIC(10,2) NOT NULL DEFAULT 0.00,
  mecanico TEXT NOT NULL,
  notas TEXT,
  creado_en TIMESTAMPTZ DEFAULT NOW()
);

-- 6. Tabla: materiales (inventario)
CREATE TABLE materiales (
  id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
  taller_id UUID NOT NULL REFERENCES talleres(id) ON DELETE CASCADE,
  nombre TEXT NOT NULL,
  descripcion TEXT,
  cantidad INTEGER NOT NULL DEFAULT 0,
  umbral_bajo INTEGER DEFAULT 5,
  precio_unitario NUMERIC(10,2) DEFAULT 0.00,
  categoria TEXT,
  proveedor TEXT,
  creado_en TIMESTAMPTZ DEFAULT NOW(),
  UNIQUE (taller_id, nombre)  -- No puede haber dos materiales con mismo nombre en el mismo taller
);

-- Índices para mejorar el rendimiento 
CREATE INDEX idx_clientes_taller ON clientes(taller_id);
CREATE INDEX idx_vehiculos_taller ON vehiculos(taller_id);
CREATE INDEX idx_citas_taller ON citas(taller_id);
CREATE INDEX idx_servicios_taller ON servicios(taller_id);
CREATE INDEX idx_materiales_taller ON materiales(taller_id);


ALTER TABLE clientes ADD COLUMN cedula TEXT UNIQUE;
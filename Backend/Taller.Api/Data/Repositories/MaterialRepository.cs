// Data/Repositories/MaterialRepository.cs
using Npgsql;
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de materiales (inventario).
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class MaterialRepository : IMaterialRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public MaterialRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene todos los materiales del taller especificado.
    /// </summary>
    public async Task<List<MaterialDto>> GetAllAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, nombre, descripcion, cantidad, umbral_bajo, precio_unitario, categoria, proveedor
            FROM materiales
            WHERE taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        var materiales = new List<MaterialDto>();
        while (await reader.ReadAsync())
        {
            materiales.Add(new MaterialDto
            {
                Id = reader.GetGuid(0),
                Nombre = reader.GetString(1),
                Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                Cantidad = reader.GetInt32(3),
                UmbralBajo = reader.GetInt32(4),
                PrecioUnitario = reader.GetDecimal(5),
                Categoria = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                Proveedor = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
            });
        }

        return materiales;
    }

    /// <summary>
    /// Obtiene un material por su ID y taller.
    /// </summary>
    public async Task<MaterialDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, nombre, descripcion, cantidad, umbral_bajo, precio_unitario, categoria, proveedor
            FROM materiales
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new MaterialDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            Cantidad = reader.GetInt32(3),
            UmbralBajo = reader.GetInt32(4),
            PrecioUnitario = reader.GetDecimal(5),
            Categoria = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Proveedor = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
        };
    }

    /// <summary>
    /// Crea un nuevo material en la base de datos.
    /// </summary>
    public async Task<MaterialDto> CreateAsync(CreateMaterialRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO materiales (nombre, descripcion, cantidad, umbral_bajo, precio_unitario, categoria, proveedor, taller_id) 
            VALUES (@nombre, @descripcion, @cantidad, @umbralBajo, @precioUnitario, @categoria, @proveedor, @tallerId) 
            RETURNING id, nombre, descripcion, cantidad, umbral_bajo, precio_unitario, categoria, proveedor", conn);

        cmd.Parameters.AddWithValue("@nombre", request.Nombre);
        cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        cmd.Parameters.AddWithValue("@cantidad", request.Cantidad);
        cmd.Parameters.AddWithValue("@umbralBajo", request.UmbralBajo);
        cmd.Parameters.AddWithValue("@precioUnitario", request.PrecioUnitario);
        cmd.Parameters.AddWithValue("@categoria", request.Categoria);
        cmd.Parameters.AddWithValue("@proveedor", request.Proveedor);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new MaterialDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            Cantidad = reader.GetInt32(3),
            UmbralBajo = reader.GetInt32(4),
            PrecioUnitario = reader.GetDecimal(5),
            Categoria = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Proveedor = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
        };
    }

    /// <summary>
    /// Actualiza un material existente en la base de datos.
    /// </summary>
    public async Task<MaterialDto?> UpdateAsync(Guid id, UpdateMaterialRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            UPDATE materiales 
            SET nombre = @nombre";

        if (!string.IsNullOrEmpty(request.Descripcion))
            sql += ", descripcion = @descripcion";
        if (request.Cantidad.HasValue)
            sql += ", cantidad = @cantidad";
        if (request.UmbralBajo.HasValue)
            sql += ", umbral_bajo = @umbralBajo";
        if (request.PrecioUnitario.HasValue)
            sql += ", precio_unitario = @precioUnitario";
        if (!string.IsNullOrEmpty(request.Categoria))
            sql += ", categoria = @categoria";
        if (!string.IsNullOrEmpty(request.Proveedor))
            sql += ", proveedor = @proveedor";

        sql += @"
            WHERE id = @id AND taller_id = @tallerId
            RETURNING id, nombre, descripcion, cantidad, umbral_bajo, precio_unitario, categoria, proveedor";

        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@nombre", request.Nombre ?? string.Empty);
        if (!string.IsNullOrEmpty(request.Descripcion))
            cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        if (request.Cantidad.HasValue)
            cmd.Parameters.AddWithValue("@cantidad", request.Cantidad.Value);
        if (request.UmbralBajo.HasValue)
            cmd.Parameters.AddWithValue("@umbralBajo", request.UmbralBajo.Value);
        if (request.PrecioUnitario.HasValue)
            cmd.Parameters.AddWithValue("@precioUnitario", request.PrecioUnitario.Value);
        if (!string.IsNullOrEmpty(request.Categoria))
            cmd.Parameters.AddWithValue("@categoria", request.Categoria);
        if (!string.IsNullOrEmpty(request.Proveedor))
            cmd.Parameters.AddWithValue("@proveedor", request.Proveedor);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new MaterialDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Descripcion = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            Cantidad = reader.GetInt32(3),
            UmbralBajo = reader.GetInt32(4),
            PrecioUnitario = reader.GetDecimal(5),
            Categoria = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Proveedor = reader.IsDBNull(7) ? string.Empty : reader.GetString(7)
        };
    }

    /// <summary>
    /// Elimina un material de la base de datos.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            DELETE FROM materiales 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Verifica si un material existe y pertenece al taller especificado.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM materiales 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    /// <summary>
    /// Verifica si ya existe un material con el mismo nombre en el mismo taller.
    /// </summary>
    public async Task<bool> ExistsByNombreAsync(string nombre, Guid tallerId, Guid? excludeId = null)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = excludeId.HasValue
            ? "SELECT COUNT(*) FROM materiales WHERE nombre = @nombre AND taller_id = @tallerId AND id != @excludeId"
            : "SELECT COUNT(*) FROM materiales WHERE nombre = @nombre AND taller_id = @tallerId";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@nombre", nombre);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);
        if (excludeId.HasValue)
            cmd.Parameters.AddWithValue("@excludeId", excludeId.Value);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    /// <summary>
    /// Obtiene el conteo de materiales del taller especificado.
    /// </summary>
    public async Task<int> GetCountAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM materiales 
            WHERE taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(count);
    }
}
// Data/Repositories/ServicioRepository.cs
using Npgsql;
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de servicios (historial de trabajos).
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class ServicioRepository : IServicioRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public ServicioRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene todos los servicios del taller especificado.
    /// </summary>
    public async Task<List<ServicioDto>> GetAllAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, vehiculo_id, fecha, descripcion, costo, mecanico, notas
            FROM servicios
            WHERE taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        var servicios = new List<ServicioDto>();
        while (await reader.ReadAsync())
        {
            servicios.Add(new ServicioDto
            {
                Id = reader.GetGuid(0),
                VehiculoId = reader.GetGuid(1),
                Fecha = DateOnly.FromDateTime(reader.GetDateTime(2)),
                Descripcion = reader.GetString(3),
                Costo = reader.GetDecimal(4),
                Mecanico = reader.GetString(5),
                Notas = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
            });
        }

        return servicios;
    }

    /// <summary>
    /// Obtiene un servicio por su ID y taller.
    /// </summary>
    public async Task<ServicioDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, vehiculo_id, fecha, descripcion, costo, mecanico, notas
            FROM servicios
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new ServicioDto
        {
            Id = reader.GetGuid(0),
            VehiculoId = reader.GetGuid(1),
            Fecha = DateOnly.FromDateTime(reader.GetDateTime(2)),
            Descripcion = reader.GetString(3),
            Costo = reader.GetDecimal(4),
            Mecanico = reader.GetString(5),
            Notas = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
        };
    }

    /// <summary>
    /// Crea un nuevo registro de servicio en la base de datos.
    /// </summary>
    public async Task<ServicioDto> CreateAsync(CreateServicioRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO servicios (vehiculo_id, fecha, descripcion, costo, mecanico, notas, taller_id) 
            VALUES (@vehiculoId, @fecha, @descripcion, @costo, @mecanico, @notas, @tallerId) 
            RETURNING id, vehiculo_id, fecha, descripcion, costo, mecanico, notas", conn);

        cmd.Parameters.AddWithValue("@vehiculoId", request.VehiculoId);
        cmd.Parameters.AddWithValue("@fecha", request.Fecha.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        cmd.Parameters.AddWithValue("@costo", request.Costo);
        cmd.Parameters.AddWithValue("@mecanico", request.Mecanico);
        cmd.Parameters.AddWithValue("@notas", request.Notas);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new ServicioDto
        {
            Id = reader.GetGuid(0),
            VehiculoId = reader.GetGuid(1),
            Fecha = DateOnly.FromDateTime(reader.GetDateTime(2)),
            Descripcion = reader.GetString(3),
            Costo = reader.GetDecimal(4),
            Mecanico = reader.GetString(5),
            Notas = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
        };
    }

    /// <summary>
    /// Actualiza un servicio existente en la base de datos.
    /// </summary>
    public async Task<ServicioDto?> UpdateAsync(Guid id, UpdateServicioRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            UPDATE servicios 
            SET vehiculo_id = @vehiculoId";

        if (request.Fecha.HasValue)
            sql += ", fecha = @fecha";
        if (!string.IsNullOrEmpty(request.Descripcion))
            sql += ", descripcion = @descripcion";
        if (request.Costo.HasValue)
            sql += ", costo = @costo";
        if (!string.IsNullOrEmpty(request.Mecanico))
            sql += ", mecanico = @mecanico";
        if (!string.IsNullOrEmpty(request.Notas))
            sql += ", notas = @notas";

        sql += @"
            WHERE id = @id AND taller_id = @tallerId
            RETURNING id, vehiculo_id, fecha, descripcion, costo, mecanico, notas";

        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@vehiculoId", request.VehiculoId);
        if (request.Fecha.HasValue)
            cmd.Parameters.AddWithValue("@fecha", request.Fecha.Value.ToDateTime(TimeOnly.MinValue));
        if (!string.IsNullOrEmpty(request.Descripcion))
            cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        if (request.Costo.HasValue)
            cmd.Parameters.AddWithValue("@costo", request.Costo.Value);
        if (!string.IsNullOrEmpty(request.Mecanico))
            cmd.Parameters.AddWithValue("@mecanico", request.Mecanico);
        if (!string.IsNullOrEmpty(request.Notas))
            cmd.Parameters.AddWithValue("@notas", request.Notas);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new ServicioDto
        {
            Id = reader.GetGuid(0),
            VehiculoId = reader.GetGuid(1),
            Fecha = DateOnly.FromDateTime(reader.GetDateTime(2)),
            Descripcion = reader.GetString(3),
            Costo = reader.GetDecimal(4),
            Mecanico = reader.GetString(5),
            Notas = reader.IsDBNull(6) ? string.Empty : reader.GetString(6)
        };
    }

    /// <summary>
    /// Elimina un servicio de la base de datos.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            DELETE FROM servicios 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Verifica si un servicio existe y pertenece al taller especificado.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM servicios 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }
}
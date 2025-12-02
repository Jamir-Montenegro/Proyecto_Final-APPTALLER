// Data/Repositories/CitaRepository.cs
using Npgsql;
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de citas.
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class CitaRepository : ICitaRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public CitaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene todas las citas del taller especificado.
    /// </summary>
    public async Task<List<CitaDto>> GetAllAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT c.id, c.cliente_id, c.fecha_hora, c.descripcion, c.estado,
                   cl.nombre AS cliente_nombre, cl.cedula AS cliente_cedula
            FROM citas c
            JOIN clientes cl ON c.cliente_id = cl.id
            WHERE c.taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        var citas = new List<CitaDto>();
        while (await reader.ReadAsync())
        {
            citas.Add(new CitaDto
            {
                Id = reader.GetGuid(0),
                ClienteId = reader.GetGuid(1),
                FechaHora = reader.GetDateTime(2),
                Descripcion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Estado = reader.GetString(4),
                ClienteNombre = reader.GetString(5),
                ClienteCedula = reader.GetString(6)
            });
        }

        return citas;
    }

    /// <summary>
    /// Obtiene una cita por su ID y taller.
    /// </summary>
    public async Task<CitaDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT c.id, c.cliente_id, c.fecha_hora, c.descripcion, c.estado,
                   cl.nombre AS cliente_nombre, cl.cedula AS cliente_cedula
            FROM citas c
            JOIN clientes cl ON c.cliente_id = cl.id
            WHERE c.id = @id AND c.taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new CitaDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            FechaHora = reader.GetDateTime(2),
            Descripcion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Estado = reader.GetString(4),
            ClienteNombre = reader.GetString(5),
            ClienteCedula = reader.GetString(6)
        };
    }

    /// <summary>
    /// Crea una nueva cita en la base de datos.
    /// </summary>
    public async Task<CitaDto> CreateAsync(CreateCitaRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO citas (cliente_id, fecha_hora, descripcion, estado, taller_id) 
            VALUES (@clienteId, @fechaHora, @descripcion, 'Pendiente', @tallerId) 
            RETURNING id, cliente_id, fecha_hora, descripcion, estado", conn);

        cmd.Parameters.AddWithValue("@clienteId", request.ClienteId);
        cmd.Parameters.AddWithValue("@fechaHora", request.FechaHora);
        cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new CitaDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            FechaHora = reader.GetDateTime(2),
            Descripcion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Estado = reader.GetString(4),
            ClienteNombre = string.Empty,
            ClienteCedula = string.Empty
        };
    }

    /// <summary>
    /// Actualiza una cita existente en la base de datos.
    /// </summary>
    public async Task<CitaDto?> UpdateAsync(Guid id, UpdateCitaRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = @"
            UPDATE citas 
            SET cliente_id = @clienteId";

        if (request.FechaHora.HasValue)
            sql += ", fecha_hora = @fechaHora";
        if (!string.IsNullOrEmpty(request.Descripcion))
            sql += ", descripcion = @descripcion";
        if (!string.IsNullOrEmpty(request.Estado))
            sql += ", estado = @estado";

        sql += @"
            WHERE id = @id AND taller_id = @tallerId
            RETURNING id, cliente_id, fecha_hora, descripcion, estado";

        using var cmd = new NpgsqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@clienteId", request.ClienteId);
        if (request.FechaHora.HasValue)
            cmd.Parameters.AddWithValue("@fechaHora", request.FechaHora.Value);
        if (!string.IsNullOrEmpty(request.Descripcion))
            cmd.Parameters.AddWithValue("@descripcion", request.Descripcion);
        if (!string.IsNullOrEmpty(request.Estado))
            cmd.Parameters.AddWithValue("@estado", request.Estado);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new CitaDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            FechaHora = reader.GetDateTime(2),
            Descripcion = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Estado = reader.GetString(4),
            ClienteNombre = string.Empty,
            ClienteCedula = string.Empty
        };
    }

    /// <summary>
    /// Elimina una cita de la base de datos.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            DELETE FROM citas 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Verifica si una cita existe y pertenece al taller especificado.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM citas 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    /// <summary>
    /// Obtiene el conteo de citas del taller especificado.
    /// </summary>
    public async Task<int> GetCountAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM citas 
            WHERE taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(count);
    }

    /// <summary>
    /// Obtiene el conteo de citas del taller especificado por estado.
    /// </summary>
    public async Task<int> GetCountByEstadoAsync(string estado, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM citas 
            WHERE estado = @estado AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@estado", estado);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(count);
    }
}
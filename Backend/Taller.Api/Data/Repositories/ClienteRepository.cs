
using Npgsql;
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de clientes.
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class ClienteRepository : IClienteRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public ClienteRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene todos los clientes del taller especificado.
    /// </summary>
    public async Task<List<ClienteDto>> GetAllAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, nombre, telefono, email, direccion, cedula
            FROM clientes
            WHERE taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        var clientes = new List<ClienteDto>();
        while (await reader.ReadAsync())
        {
            clientes.Add(new ClienteDto
            {
                Id = reader.GetGuid(0),
                Nombre = reader.GetString(1),
                Telefono = reader.GetString(2),
                Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                Direccion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                Cedula = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
            });
        }

        return clientes;
    }

    /// <summary>
    /// Obtiene un cliente por su ID y taller.
    /// </summary>
    public async Task<ClienteDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, nombre, telefono, email, direccion, cedula
            FROM clientes
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new ClienteDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Telefono = reader.GetString(2),
            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Direccion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
            Cedula = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
        };
    }

    /// <summary>
    /// Crea un nuevo cliente en la base de datos.
    /// </summary>
    public async Task<ClienteDto> CreateAsync(CreateClienteRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO clientes (nombre, telefono, email, direccion, cedula, taller_id) 
            VALUES (@nombre, @telefono, @email, @direccion, @cedula, @tallerId) 
            RETURNING id, nombre, telefono, email, direccion, cedula", conn);

        cmd.Parameters.AddWithValue("@nombre", request.Nombre);
        cmd.Parameters.AddWithValue("@telefono", request.Telefono);
        cmd.Parameters.AddWithValue("@email", request.Email);
        cmd.Parameters.AddWithValue("@direccion", request.Direccion);
        cmd.Parameters.AddWithValue("@cedula", request.Cedula);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new ClienteDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Telefono = reader.GetString(2),
            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Direccion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
            Cedula = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
        };
    }

    /// <summary>
    /// Actualiza un cliente existente en la base de datos.
    /// </summary>
    public async Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            UPDATE clientes 
            SET nombre = @nombre, telefono = @telefono, email = @email, 
                direccion = @direccion, cedula = @cedula
            WHERE id = @id AND taller_id = @tallerId
            RETURNING id, nombre, telefono, email, direccion, cedula", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@nombre", request.Nombre);
        cmd.Parameters.AddWithValue("@telefono", request.Telefono);
        cmd.Parameters.AddWithValue("@email", request.Email);
        cmd.Parameters.AddWithValue("@direccion", request.Direccion);
        cmd.Parameters.AddWithValue("@cedula", request.Cedula);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new ClienteDto
        {
            Id = reader.GetGuid(0),
            Nombre = reader.GetString(1),
            Telefono = reader.GetString(2),
            Email = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
            Direccion = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
            Cedula = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
        };
    }

    /// <summary>
    /// Elimina un cliente de la base de datos.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            DELETE FROM clientes 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Verifica si un cliente existe y pertenece al taller especificado.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM clientes 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    /// <summary>
    /// Verifica si ya existe un cliente con la misma cédula en el mismo taller.
    /// </summary>
    public async Task<bool> ExistsByCedulaAsync(string cedula, Guid tallerId, Guid? excludeId = null)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = excludeId.HasValue
            ? "SELECT COUNT(*) FROM clientes WHERE cedula = @cedula AND taller_id = @tallerId AND id != @excludeId"
            : "SELECT COUNT(*) FROM clientes WHERE cedula = @cedula AND taller_id = @tallerId";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@cedula", cedula);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);
        if (excludeId.HasValue)
            cmd.Parameters.AddWithValue("@excludeId", excludeId.Value);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    public async Task<int> GetCountAsync(Guid tallerId)
{
    using var conn = new NpgsqlConnection(_connectionString);
    await conn.OpenAsync();

    using var cmd = new NpgsqlCommand(@"
        SELECT COUNT(*) FROM clientes 
        WHERE taller_id = @tallerId", conn);

    cmd.Parameters.AddWithValue("@tallerId", tallerId);

    var count = await cmd.ExecuteScalarAsync();
    return Convert.ToInt32(count);
}
}
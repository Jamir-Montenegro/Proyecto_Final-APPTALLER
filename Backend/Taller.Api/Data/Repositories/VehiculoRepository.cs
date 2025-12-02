
using Npgsql;
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de vehículos.
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class VehiculoRepository : IVehiculoRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public VehiculoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene todos los vehículos del taller especificado.
    /// </summary>
    public async Task<List<VehiculoDto>> GetAllAsync(Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT v.id, v.cliente_id, v.marca, v.modelo, v.anio, v.placa, v.color, v.vin,
                   c.nombre AS cliente_nombre, c.cedula AS cliente_cedula
            FROM vehiculos v
            JOIN clientes c ON v.cliente_id = c.id
            WHERE v.taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        var vehiculos = new List<VehiculoDto>();
        while (await reader.ReadAsync())
        {
            vehiculos.Add(new VehiculoDto
            {
                Id = reader.GetGuid(0),
                ClienteId = reader.GetGuid(1),
                Marca = reader.GetString(2),
                Modelo = reader.GetString(3),
                Anio = reader.GetInt32(4),
                Placa = reader.GetString(5),
                Color = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                Vin = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                ClienteNombre = reader.GetString(8),
                ClienteCedula = reader.GetString(9)
            });
        }

        return vehiculos;
    }

    /// <summary>
    /// Obtiene un vehículo por su ID y taller.
    /// </summary>
    public async Task<VehiculoDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT v.id, v.cliente_id, v.marca, v.modelo, v.anio, v.placa, v.color, v.vin,
                   c.nombre AS cliente_nombre, c.cedula AS cliente_cedula
            FROM vehiculos v
            JOIN clientes c ON v.cliente_id = c.id
            WHERE v.id = @id AND v.taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new VehiculoDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            Marca = reader.GetString(2),
            Modelo = reader.GetString(3),
            Anio = reader.GetInt32(4),
            Placa = reader.GetString(5),
            Color = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Vin = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
            ClienteNombre = reader.GetString(8),
            ClienteCedula = reader.GetString(9)
        };
    }

    /// <summary>
    /// Crea un nuevo vehículo en la base de datos.
    /// </summary>
    public async Task<VehiculoDto> CreateAsync(CreateVehiculoRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            INSERT INTO vehiculos (cliente_id, marca, modelo, anio, placa, color, vin, taller_id) 
            VALUES (@clienteId, @marca, @modelo, @anio, @placa, @color, @vin, @tallerId) 
            RETURNING id, cliente_id, marca, modelo, anio, placa, color, vin", conn);

        cmd.Parameters.AddWithValue("@clienteId", request.ClienteId);
        cmd.Parameters.AddWithValue("@marca", request.Marca);
        cmd.Parameters.AddWithValue("@modelo", request.Modelo);
        cmd.Parameters.AddWithValue("@anio", request.Anio);
        cmd.Parameters.AddWithValue("@placa", request.Placa);
        cmd.Parameters.AddWithValue("@color", request.Color);
        cmd.Parameters.AddWithValue("@vin", request.Vin);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        await reader.ReadAsync();

        return new VehiculoDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            Marca = reader.GetString(2),
            Modelo = reader.GetString(3),
            Anio = reader.GetInt32(4),
            Placa = reader.GetString(5),
            Color = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Vin = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
            // ClienteNombre y ClienteCedula se obtendrían en otro paso o en el servicio
            ClienteNombre = string.Empty,
            ClienteCedula = string.Empty
        };
    }

    /// <summary>
    /// Actualiza un vehículo existente en la base de datos.
    /// </summary>
    public async Task<VehiculoDto?> UpdateAsync(Guid id, UpdateVehiculoRequest request, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            UPDATE vehiculos 
            SET cliente_id = @clienteId, marca = @marca, modelo = @modelo, anio = @anio, 
                placa = @placa, color = @color, vin = @vin
            WHERE id = @id AND taller_id = @tallerId
            RETURNING id, cliente_id, marca, modelo, anio, placa, color, vin", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@clienteId", request.ClienteId);
        cmd.Parameters.AddWithValue("@marca", request.Marca);
        cmd.Parameters.AddWithValue("@modelo", request.Modelo);
        cmd.Parameters.AddWithValue("@anio", request.Anio);
        cmd.Parameters.AddWithValue("@placa", request.Placa);
        cmd.Parameters.AddWithValue("@color", request.Color);
        cmd.Parameters.AddWithValue("@vin", request.Vin);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        return new VehiculoDto
        {
            Id = reader.GetGuid(0),
            ClienteId = reader.GetGuid(1),
            Marca = reader.GetString(2),
            Modelo = reader.GetString(3),
            Anio = reader.GetInt32(4),
            Placa = reader.GetString(5),
            Color = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
            Vin = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
            ClienteNombre = string.Empty,
            ClienteCedula = string.Empty
        };
    }

    /// <summary>
    /// Elimina un vehículo de la base de datos.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            DELETE FROM vehiculos 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
    }

    /// <summary>
    /// Verifica si un vehículo existe y pertenece al taller especificado.
    /// </summary>
    public async Task<bool> ExistsAsync(Guid id, Guid tallerId)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM vehiculos 
            WHERE id = @id AND taller_id = @tallerId", conn);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@tallerId", tallerId);

        var count = await cmd.ExecuteScalarAsync();
        return Convert.ToInt64(count) > 0;
    }

    /// <summary>
    /// Verifica si ya existe un vehículo con la misma placa en el mismo taller.
    /// </summary>
    public async Task<bool> ExistsByPlacaAsync(string placa, Guid tallerId, Guid? excludeId = null)
    {
        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var sql = excludeId.HasValue
            ? "SELECT COUNT(*) FROM vehiculos WHERE placa = @placa AND taller_id = @tallerId AND id != @excludeId"
            : "SELECT COUNT(*) FROM vehiculos WHERE placa = @placa AND taller_id = @tallerId";

        using var cmd = new NpgsqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@placa", placa);
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
        SELECT COUNT(*) FROM vehiculos 
        WHERE taller_id = @tallerId", conn);

    cmd.Parameters.AddWithValue("@tallerId", tallerId);

    var count = await cmd.ExecuteScalarAsync();
    return Convert.ToInt32(count);
}
}
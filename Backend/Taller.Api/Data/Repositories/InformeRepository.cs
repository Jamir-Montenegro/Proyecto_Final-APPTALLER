// Data/Repositories/InformeRepository.cs
using Npgsql;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de informe (dashboard).
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class InformeRepository : IInformeRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public InformeRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Obtiene el conteo de vehículos del taller especificado.
    /// </summary>
    public async Task<int> GetVehiculosCountAsync(Guid tallerId)
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

    /// <summary>
    /// Obtiene el conteo de clientes del taller especificado.
    /// </summary>
    public async Task<int> GetClientesCountAsync(Guid tallerId)
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

    /// <summary>
    /// Obtiene el conteo total de citas del taller especificado.
    /// </summary>
    public async Task<int> GetCitasCountAsync(Guid tallerId)
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
    public async Task<int> GetCitasCountByEstadoAsync(string estado, Guid tallerId)
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
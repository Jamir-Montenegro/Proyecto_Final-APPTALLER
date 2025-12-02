// Data/Repositories/AuthRepository.cs
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using Taller.Api.Models;
using Microsoft.Extensions.Configuration; 
namespace Taller.Api.Data.Repositories;

/// <summary>
/// Implementación del repositorio de autenticación.
/// Se comunica directamente con la base de datos de Supabase (PostgreSQL).
/// </summary>
public class AuthRepository : IAuthRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Constructor del repositorio.
    /// </summary>
    /// <param name="connectionString">Cadena de conexión a la base de datos.</param>
    public AuthRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Intenta iniciar sesión con las credenciales proporcionadas.
    /// </summary>
    public async Task<UserResponse?> LoginAsync(LoginRequest request)
    {
        Console.WriteLine(">>> PASSWORD RECIBIDO LOGIN: " + request.Password);

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var cmd = new NpgsqlCommand(@"
            SELECT id, nombre, email, password_hash 
            FROM talleres 
            WHERE email = @email", conn);

        cmd.Parameters.AddWithValue("@email", request.Email);

        using var reader = await cmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;
        var storedHash = reader.GetString(3);
        var inputHash = HashPassword(request.Password);

if (storedHash != inputHash) return null;

        var id = reader.GetFieldValue<Guid>(reader.GetOrdinal("id"));
        var nombre = reader.GetString(reader.GetOrdinal("nombre"));
        var email = reader.GetString(reader.GetOrdinal("email"));


        var token = Guid.NewGuid().ToString();

        return new UserResponse
        {
            Id = id,
            Nombre = nombre ?? "",
            Email = email ?? "",
            Token = token ?? ""
        };
    }

    /// <summary>
    /// Registra un nuevo taller en la base de datos.
    /// </summary>
    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            return null;

        using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        using var checkCmd = new NpgsqlCommand(@"
            SELECT COUNT(*) FROM talleres WHERE email = @email", conn);
        checkCmd.Parameters.AddWithValue("@email", request.Email);
        var count = await checkCmd.ExecuteScalarAsync();
        var exists = Convert.ToInt64(count) > 0;
        if (exists) return null;

        var hash = HashPassword(request.Password);

        using var insertCmd = new NpgsqlCommand(@"
            INSERT INTO talleres (nombre, email, password_hash) 
            VALUES (@nombre, @email, @hash) 
            RETURNING id, nombre, email", conn);

        insertCmd.Parameters.AddWithValue("@nombre", request.Nombre);
        insertCmd.Parameters.AddWithValue("@email", request.Email);
        insertCmd.Parameters.AddWithValue("@hash", hash);

        using var reader = await insertCmd.ExecuteReaderAsync();
        if (!await reader.ReadAsync()) return null;

        // Usa GetFieldValue<Guid> para leer el UUID correctamente
       var id = reader.GetFieldValue<Guid>(reader.GetOrdinal("id"));
       var nombre = reader.GetString(reader.GetOrdinal("nombre"));
       var email = reader.GetString(reader.GetOrdinal("email"));

 

        var token = Guid.NewGuid().ToString();

            Console.WriteLine(">>> USER RESPONSE:");
        Console.WriteLine("ID: " + id);
        Console.WriteLine("Nombre: " + nombre);
        Console.WriteLine("Email: " + email);
        Console.WriteLine("Token: " + token);


        return new UserResponse
        {
            Id = id,
            Nombre = nombre ?? "",
            Email = email ?? "",
            Token = token ?? ""
        };
    }

    /// <summary>
    /// Genera un hash SHA256 de una contraseña.
    /// NOTA: En producción, usa BCrypt para mayor seguridad.
    /// </summary>
   private string HashPassword(string password)
{
    if (string.IsNullOrWhiteSpace(password))
    {
        Console.WriteLine(">>> ERROR: PASSWORD ES NULL EN HashPassword");
        return string.Empty; // evita explotar
    }

    using var sha256 = SHA256.Create();
    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
    return Convert.ToBase64String(bytes);
}
   
    
}

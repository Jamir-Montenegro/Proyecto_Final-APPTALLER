
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de autenticación en la base de datos.
/// </summary>
public interface IAuthRepository
{
    /// <summary>
    /// Intenta iniciar sesión con las credenciales proporcionadas.
    /// </summary>
    /// <param name="request">Los datos de login (email y contraseña).</param>
    /// <returns>Los datos del taller y un token si las credenciales son válidas; null en caso contrario.</returns>
    Task<UserResponse?> LoginAsync(LoginRequest request);

    /// <summary>
    /// Registra un nuevo taller en la base de datos.
    /// </summary>
    /// <param name="request">Los datos del nuevo taller.</param>
    /// <returns>Los datos del nuevo taller y un token; null si el email ya existe.</returns>
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
}

namespace Taller.Api.Models;

/// <summary>
/// Representa la respuesta de autenticación exitosa.
/// Contiene los datos básicos del taller y el token de acceso.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Obtiene o establece el identificador único del taller.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///  establece el nombre del taller.
    /// </summary>
    /// <example>Taller de Chipstería</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    ///  establece el correo electrónico del taller.
    /// </summary>
    /// <example>contacto@chipsteria.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    ///  establece el token de acceso JWT.
    /// Este token debe ser incluido en las peticiones futuras para autenticar al usuario.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...</example>
    public string Token { get; set; } = string.Empty;
}
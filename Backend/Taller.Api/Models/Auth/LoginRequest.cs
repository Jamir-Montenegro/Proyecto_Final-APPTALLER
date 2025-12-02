
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud de inicio de sesión de un taller.
/// Contiene las credenciales necesarias para autenticar al usuario.
/// </summary>
public class LoginRequest
{
    /// <summary>
    /// Obtiene o establece el correo electrónico del taller.
    /// </summary>
    /// <example>ejemplo@taller.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la contraseña del taller.
    /// </summary>
    /// <example>ContraseñaSegura123!</example>
    public string Password { get; set; } = string.Empty;
}
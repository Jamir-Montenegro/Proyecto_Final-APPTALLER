
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud de registro de un nuevo taller.
/// Contiene los datos básicos para crear una cuenta.
/// </summary>
public class RegisterRequest
{
    /// <summary>
    /// establece el nombre del taller.
    /// </summary>
    /// <example>Taller de Chipstería</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene el correo electrónico del taller.
    /// Debe ser único en el sistema.
    /// </summary>
    /// <example>contacto@chipsteria.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// establece la contraseña del taller.
    /// </summary>
    /// <example>ContraseñaSegura123!</example>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// establece la confirmación de la contraseña.
    /// Debe coincidir con <see cref="Password"/>.
    /// </summary>
    /// <example>ContraseñaSegura123!</example>
    public string? ConfirmPassword { get; set; } = string.Empty;
}
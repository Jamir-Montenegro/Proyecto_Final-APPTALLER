
namespace Taller.Api.Models;

/// <summary>
/// Representa un cliente en el sistema.
/// Contiene la información de contacto y de identificación del cliente.
/// </summary>
public class ClienteDto
{
    /// <summary>
    /// Obtiene o establece el identificador único del cliente.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del cliente.
    /// </summary>
    /// <example>Juan Pérez</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el número de teléfono del cliente.
    /// </summary>
    /// <example>+1-123-456-7890</example>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el correo electrónico del cliente.
    /// </summary>
    /// <example>juan.perez@email.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la dirección del cliente.
    /// </summary>
    /// <example>Calle Falsa 123, Ciudad</example>
    public string Direccion { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el número de cédula del cliente.
    /// Este campo es único por taller y se usa para diferenciar clientes con el mismo nombre.
    /// </summary>
    /// <example>123-456-7890-X</example>
    public string Cedula { get; set; } = string.Empty;
}
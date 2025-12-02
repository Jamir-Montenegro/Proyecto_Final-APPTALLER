// Models/Citas/CitaDto.cs
namespace Taller.Api.Models;

/// <summary>
/// Representa una cita en el sistema.
/// Incluye información de la cita, del cliente y del estado actual.
/// </summary>
public class CitaDto
{
    /// <summary>
    /// Obtiene o establece el identificador único de la cita.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del cliente asociado a la cita.
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del cliente asociado a la cita.
    /// Se incluye para facilitar la visualización en el frontend.
    /// </summary>
    /// <example>Juan Pérez</example>
    public string ClienteNombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el número de cédula del cliente asociado a la cita.
    /// Se incluye para diferenciar clientes con el mismo nombre.
    /// </summary>
    /// <example>123-456-7890-X</example>
    public string ClienteCedula { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la fecha y hora de la cita.
    /// </summary>
    /// <example>2025-12-15T10:00:00</example>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción o motivo de la cita.
    /// </summary>
    /// <example>Revisión general del vehículo</example>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el estado actual de la cita.
    /// Valores posibles: 'Pendiente', 'Atendida', 'Cancelada'.
    /// </summary>
    /// <example>Pendiente</example>
    public string Estado { get; set; } = "Pendiente";
}
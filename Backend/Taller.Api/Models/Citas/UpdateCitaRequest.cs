// Models/Citas/UpdateCitaRequest.cs
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud para actualizar una cita existente.
/// Contiene los datos que se pueden modificar de una cita ya registrada.
/// </summary>
public class UpdateCitaRequest
{
    /// <summary>
    /// Obtiene o establece el identificador del cliente asociado a la cita.
    /// Este cliente debe existir previamente en el sistema.
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora de la cita.
    /// Puede actualizarse si se reprograma la cita.
    /// </summary>
    /// <example>2025-12-15T10:00:00</example>
    public DateTime? FechaHora { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción o motivo de la cita.
    /// </summary>
    /// <example>Revisión general del vehículo</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Obtiene o establece el estado actual de la cita.
    /// Valores posibles: 'Pendiente', 'Atendida', 'Cancelada'.
    /// Este es el campo que más frecuentemente se actualiza desde el panel del taller.
    /// </summary>
    /// <example>Atendida</example>
    public string? Estado { get; set; }
}
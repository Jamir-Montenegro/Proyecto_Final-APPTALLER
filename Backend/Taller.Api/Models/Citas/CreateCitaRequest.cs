// Models/Citas/CreateCitaRequest.cs
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud para crear una nueva cita.
/// Contiene los datos necesarios para registrar una cita en el sistema.
/// </summary>
public class CreateCitaRequest
{
    /// <summary>
    /// Obtiene o establece el identificador del cliente que solicita la cita.
    /// Este cliente debe existir previamente en el sistema.
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha y hora de la cita solicitada.
    /// </summary>
    /// <example>2025-12-15T10:00:00</example>
    public DateTime FechaHora { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción o motivo de la cita.
    /// </summary>
    /// <example>Revisión general del vehículo</example>
    public string Descripcion { get; set; } = string.Empty;
}
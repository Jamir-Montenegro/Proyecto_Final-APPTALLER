
namespace Taller.Api.Models;

/// <summary>
/// Representa un registro de servicio (trabajo realizado) en el historial del taller.
/// Incluye información sobre el trabajo, fecha y costos.
/// </summary>
public class ServicioDto
{
    /// <summary>
    /// Obtiene o establece el identificador único del servicio.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del vehículo al que se le realizó el servicio.
    /// </summary>
    public Guid VehiculoId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha en que se realizó el servicio.
    /// </summary>
    /// <example>2025-03-15</example>
    public DateOnly Fecha { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción del trabajo realizado.
    /// </summary>
    /// <example>Cambio de aceite y filtro</example>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el costo total del servicio.
    /// </summary>
    /// <example>150.50</example>
    public decimal Costo { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del mecánico que realizó el trabajo.
    /// </summary>
    /// <example>Carlos Méndez</example>
    public string Mecanico { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece notas adicionales sobre el servicio.
    /// </summary>
    /// <example>Se recomendó revisión de frenos en 6 meses.</example>
    public string Notas { get; set; } = string.Empty;
}
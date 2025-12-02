
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud para actualizar un servicio existente en el historial.
/// Contiene los datos que se pueden modificar de un registro de servicio ya creado.
/// </summary>
public class UpdateServicioRequest
{
    /// <summary>
    /// Obtiene o establece el identificador del vehículo al que se le realizó el servicio.
    /// Este vehículo debe existir previamente en el sistema.
    /// </summary>
    public Guid VehiculoId { get; set; }

    /// <summary>
    /// Obtiene o establece la fecha en que se realizó el servicio.
    /// </summary>
    /// <example>2025-03-15</example>
    public DateOnly? Fecha { get; set; }

    /// <summary>
    /// Obtiene o establece la descripción del trabajo realizado.
    /// </summary>
    /// <example>Cambio de aceite y filtro</example>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Obtiene o establece el costo total del servicio.
    /// </summary>
    /// <example>150.50</example>
    public decimal? Costo { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del mecánico que realizó el trabajo.
    /// </summary>
    /// <example>Carlos Méndez</example>
    public string? Mecanico { get; set; }

    /// <summary>
    /// Obtiene o establece notas adicionales sobre el servicio.
    /// </summary>
    /// <example>Se recomendó revisión de frenos en 6 meses.</example>
    public string? Notas { get; set; }
}
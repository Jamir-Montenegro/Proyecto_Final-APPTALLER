
namespace Taller.Api.Models;

/// <summary>
/// Representa el informe general (dashboard) del taller.
/// Contiene métricas clave y estadísticas resumidas.
/// </summary>
public class InformeDto
{
    /// <summary>
    /// Obtiene o establece el número total de vehículos registrados en el taller.
    /// </summary>
    /// <example>42</example>
    public int VehiculosRegistrados { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de clientes registrados en el taller.
    /// </summary>
    /// <example>35</example>
    public int ClientesRegistrados { get; set; }

    /// <summary>
    /// Obtiene o establece el número total de citas solicitadas.
    /// </summary>
    /// <example>60</example>
    public int CitasSolicitadas { get; set; }

    /// <summary>
    /// Obtiene o establece el número de citas con estado 'Atendida'.
    /// </summary>
    /// <example>45</example>
    public int CitasAtendidas { get; set; }

    /// <summary>
    /// Obtiene o establece el número de citas con estado 'Pendiente'.
    /// </summary>
    /// <example>10</example>
    public int CitasPendientes { get; set; }

    /// <summary>
    /// Obtiene o establece el número de citas con estado 'Cancelada'.
    /// </summary>
    /// <example>5</example>
    public int CitasCanceladas { get; set; }

    /// <summary>
    /// Obtiene o establece la tasa de citas completadas (porcentaje).
    /// Calculado como (CitasAtendidas / CitasSolicitadas) * 100.
    /// </summary>
    /// <example>75.00</example>
    public double TasaCitasCompletadas { get; set; }
}
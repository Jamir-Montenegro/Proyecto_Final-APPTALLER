
namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de informe (dashboard) en la base de datos.
/// </summary>
public interface IInformeRepository
{
    /// <summary>
    /// Obtiene el conteo de vehículos del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número total de vehículos.</returns>
    Task<int> GetVehiculosCountAsync(Guid tallerId);

    /// <summary>
    /// Obtiene el conteo de clientes del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número total de clientes.</returns>
    Task<int> GetClientesCountAsync(Guid tallerId);

    /// <summary>
    /// Obtiene el conteo total de citas del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número total de citas.</returns>
    Task<int> GetCitasCountAsync(Guid tallerId);

    /// <summary>
    /// Obtiene el conteo de citas del taller especificado por estado.
    /// </summary>
    /// <param name="estado">El estado de las citas a contar (ej: 'Pendiente', 'Atendida').</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número de citas con el estado especificado.</returns>
    Task<int> GetCitasCountByEstadoAsync(string estado, Guid tallerId);
}
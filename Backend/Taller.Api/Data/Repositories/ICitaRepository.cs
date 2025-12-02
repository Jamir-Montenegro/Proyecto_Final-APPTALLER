
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de citas en la base de datos.
/// </summary>
public interface ICitaRepository
{
    /// <summary>
    /// Obtiene todas las citas del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Una lista de citas.</returns>
    Task<List<CitaDto>> GetAllAsync(Guid tallerId);

    /// <summary>
    /// Obtiene una cita por su ID y taller.
    /// </summary>
    /// <param name="id">El ID de la cita.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos de la cita o null si no se encuentra.</returns>
    Task<CitaDto?> GetByIdAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Crea una nueva cita en la base de datos.
    /// </summary>
    /// <param name="request">Los datos de la nueva cita.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos de la cita creada.</returns>
    Task<CitaDto> CreateAsync(CreateCitaRequest request, Guid tallerId);

    /// <summary>
    /// Actualiza una cita existente en la base de datos.
    /// </summary>
    /// <param name="id">El ID de la cita a actualizar.</param>
    /// <param name="request">Los nuevos datos de la cita.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos actualizados de la cita o null si no se encuentra.</returns>
    Task<CitaDto?> UpdateAsync(Guid id, UpdateCitaRequest request, Guid tallerId);

    /// <summary>
    /// Elimina una cita de la base de datos.
    /// </summary>
    /// <param name="id">El ID de la cita a eliminar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
    Task<bool> DeleteAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si una cita existe y pertenece al taller especificado.
    /// </summary>
    /// <param name="id">El ID de la cita.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Obtiene el conteo de citas del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número total de citas.</returns>
    Task<int> GetCountAsync(Guid tallerId);

    /// <summary>
    /// Obtiene el conteo de citas del taller especificado por estado.
    /// </summary>
    /// <param name="estado">El estado de las citas a contar (ej: 'Pendiente', 'Atendida').</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número de citas con el estado especificado.</returns>
    Task<int> GetCountByEstadoAsync(string estado, Guid tallerId);
}
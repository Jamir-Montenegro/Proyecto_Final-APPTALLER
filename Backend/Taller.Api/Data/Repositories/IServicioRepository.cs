
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de servicios (historial de trabajos) en la base de datos.
/// </summary>
public interface IServicioRepository
{
    /// <summary>
    /// Obtiene todos los servicios del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Una lista de servicios.</returns>
    Task<List<ServicioDto>> GetAllAsync(Guid tallerId);

    /// <summary>
    /// Obtiene un servicio por su ID y taller.
    /// </summary>
    /// <param name="id">El ID del servicio.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del servicio o null si no se encuentra.</returns>
    Task<ServicioDto?> GetByIdAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Crea un nuevo registro de servicio en la base de datos.
    /// </summary>
    /// <param name="request">Los datos del nuevo servicio.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del servicio creado.</returns>
    Task<ServicioDto> CreateAsync(CreateServicioRequest request, Guid tallerId);

    /// <summary>
    /// Actualiza un servicio existente en la base de datos.
    /// </summary>
    /// <param name="id">El ID del servicio a actualizar.</param>
    /// <param name="request">Los nuevos datos del servicio.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos actualizados del servicio o null si no se encuentra.</returns>
    Task<ServicioDto?> UpdateAsync(Guid id, UpdateServicioRequest request, Guid tallerId);

    /// <summary>
    /// Elimina un servicio de la base de datos.
    /// </summary>
    /// <param name="id">El ID del servicio a eliminar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
    Task<bool> DeleteAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si un servicio existe y pertenece al taller especificado.
    /// </summary>
    /// <param name="id">El ID del servicio.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id, Guid tallerId);
}
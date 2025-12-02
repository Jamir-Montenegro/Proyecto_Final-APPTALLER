
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de vehículos en la base de datos.
/// </summary>
public interface IVehiculoRepository
{
    /// <summary>
    /// Obtiene todos los vehículos del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Una lista de vehículos.</returns>
    Task<List<VehiculoDto>> GetAllAsync(Guid tallerId);

    /// <summary>
    /// Obtiene un vehículo por su ID y taller.
    /// </summary>
    /// <param name="id">El ID del vehículo.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del vehículo o null si no se encuentra.</returns>
    Task<VehiculoDto?> GetByIdAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Crea un nuevo vehículo en la base de datos.
    /// </summary>
    /// <param name="request">Los datos del nuevo vehículo.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del vehículo creado.</returns>
    Task<VehiculoDto> CreateAsync(CreateVehiculoRequest request, Guid tallerId);

    /// <summary>
    /// Actualiza un vehículo existente en la base de datos.
    /// </summary>
    /// <param name="id">El ID del vehículo a actualizar.</param>
    /// <param name="request">Los nuevos datos del vehículo.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos actualizados del vehículo o null si no se encuentra.</returns>
    Task<VehiculoDto?> UpdateAsync(Guid id, UpdateVehiculoRequest request, Guid tallerId);

    /// <summary>
    /// Elimina un vehículo de la base de datos.
    /// </summary>
    /// <param name="id">El ID del vehículo a eliminar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
    Task<bool> DeleteAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si un vehículo existe y pertenece al taller especificado.
    /// </summary>
    /// <param name="id">El ID del vehículo.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si ya existe un vehículo con la misma placa en el mismo taller.
    /// </summary>
    /// <param name="placa">La placa a verificar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <param name="excludeId">ID del vehículo a excluir de la verificación (para actualizaciones).</param>
    /// <returns>True si existe otro vehículo con esa placa, false en caso contrario.</returns>
    Task<bool> ExistsByPlacaAsync(string placa, Guid tallerId, Guid? excludeId = null);


    Task<int> GetCountAsync(Guid tallerId);
}
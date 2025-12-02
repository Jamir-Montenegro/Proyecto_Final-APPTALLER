// Data/Repositories/IMaterialRepository.cs
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de materiales (inventario) en la base de datos.
/// </summary>
public interface IMaterialRepository
{
    /// <summary>
    /// Obtiene todos los materiales del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Una lista de materiales.</returns>
    Task<List<MaterialDto>> GetAllAsync(Guid tallerId);

    /// <summary>
    /// Obtiene un material por su ID y taller.
    /// </summary>
    /// <param name="id">El ID del material.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del material o null si no se encuentra.</returns>
    Task<MaterialDto?> GetByIdAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Crea un nuevo material en la base de datos.
    /// </summary>
    /// <param name="request">Los datos del nuevo material.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del material creado.</returns>
    Task<MaterialDto> CreateAsync(CreateMaterialRequest request, Guid tallerId);

    /// <summary>
    /// Actualiza un material existente en la base de datos.
    /// </summary>
    /// <param name="id">El ID del material a actualizar.</param>
    /// <param name="request">Los nuevos datos del material.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos actualizados del material o null si no se encuentra.</returns>
    Task<MaterialDto?> UpdateAsync(Guid id, UpdateMaterialRequest request, Guid tallerId);

    /// <summary>
    /// Elimina un material de la base de datos.
    /// </summary>
    /// <param name="id">El ID del material a eliminar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
    Task<bool> DeleteAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si un material existe y pertenece al taller especificado.
    /// </summary>
    /// <param name="id">El ID del material.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si ya existe un material con el mismo nombre en el mismo taller.
    /// </summary>
    /// <param name="nombre">El nombre del material a verificar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <param name="excludeId">ID del material a excluir de la verificación (para actualizaciones).</param>
    /// <returns>True si existe otro material con ese nombre, false en caso contrario.</returns>
    Task<bool> ExistsByNombreAsync(string nombre, Guid tallerId, Guid? excludeId = null);

    /// <summary>
    /// Obtiene el conteo de materiales del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>El número total de materiales.</returns>
    Task<int> GetCountAsync(Guid tallerId);
}
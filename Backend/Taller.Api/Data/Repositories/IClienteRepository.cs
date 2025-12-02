
using Taller.Api.Models;

namespace Taller.Api.Data.Repositories;

/// <summary>
/// Define los métodos para operaciones de clientes en la base de datos.
/// </summary>
public interface IClienteRepository
{
    /// <summary>
    /// Obtiene todos los clientes del taller especificado.
    /// </summary>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Una lista de clientes.</returns>
    Task<List<ClienteDto>> GetAllAsync(Guid tallerId);

    /// <summary>
    /// Obtiene un cliente por su ID y taller.
    /// </summary>
    /// <param name="id">El ID del cliente.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del cliente o null si no se encuentra.</returns>
    Task<ClienteDto?> GetByIdAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Crea un nuevo cliente en la base de datos.
    /// </summary>
    /// <param name="request">Los datos del nuevo cliente.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos del cliente creado.</returns>
    Task<ClienteDto> CreateAsync(CreateClienteRequest request, Guid tallerId);

    /// <summary>
    /// Actualiza un cliente existente en la base de datos.
    /// </summary>
    /// <param name="id">El ID del cliente a actualizar.</param>
    /// <param name="request">Los nuevos datos del cliente.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>Los datos actualizados del cliente o null si no se encuentra.</returns>
    Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteRequest request, Guid tallerId);

    /// <summary>
    /// Elimina un cliente de la base de datos.
    /// </summary>
    /// <param name="id">El ID del cliente a eliminar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario.</returns>
    Task<bool> DeleteAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si un cliente existe y pertenece al taller especificado.
    /// </summary>
    /// <param name="id">El ID del cliente.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <returns>True si existe, false en caso contrario.</returns>
    Task<bool> ExistsAsync(Guid id, Guid tallerId);

    /// <summary>
    /// Verifica si ya existe un cliente con la misma cédula en el mismo taller.
    /// </summary>
    /// <param name="cedula">La cédula a verificar.</param>
    /// <param name="tallerId">El ID del taller autenticado.</param>
    /// <param name="excludeId">ID del cliente a excluir de la verificación (para actualizaciones).</param>
    /// <returns>True si existe otro cliente con esa cédula, false en caso contrario.</returns>
    Task<bool> ExistsByCedulaAsync(string cedula, Guid tallerId, Guid? excludeId = null);


    // Agrega este método dentro de la interfaz
     Task<int> GetCountAsync(Guid tallerId);
}


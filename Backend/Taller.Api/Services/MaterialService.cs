// Services/MaterialService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class MaterialService
{
    private readonly IMaterialRepository _materialRepository;

    // Constructor para inyección de dependencias
    public MaterialService(IMaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    /// <summary>
    /// Obtiene todos los materiales del taller especificado.
    /// </summary>
    public async Task<List<MaterialDto>> GetAllAsync(Guid tallerId)
    {
        return await _materialRepository.GetAllAsync(tallerId);
    }

    /// <summary>
    /// Obtiene un material por su ID y taller.
    /// </summary>
    public async Task<MaterialDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        return await _materialRepository.GetByIdAsync(id, tallerId);
    }

    /// <summary>
    /// Crea un nuevo material.
    /// </summary>
    public async Task<MaterialDto> CreateAsync(CreateMaterialRequest request, Guid tallerId)
    {
        // Validación 1: El nombre del material debe ser único por taller
        if (!string.IsNullOrWhiteSpace(request.Nombre))
        {
            var nombreExiste = await _materialRepository.ExistsByNombreAsync(request.Nombre, tallerId);
            if (nombreExiste)
            {
                throw new ArgumentException("Ya existe un material con ese nombre en este taller.");
            }
        }

        // Validación 2: La cantidad no puede ser negativa
        if (request.Cantidad < 0)
        {
            throw new ArgumentException("La cantidad no puede ser negativa.");
        }

        // Si todas las validaciones pasan, crea el material
        return await _materialRepository.CreateAsync(request, tallerId);
    }

    /// <summary>
    /// Actualiza un material existente.
    /// </summary>
    public async Task<MaterialDto?> UpdateAsync(Guid id, UpdateMaterialRequest request, Guid tallerId)
    {
        // Validación 1: Verificar que el material exista y pertenezca al taller
        var materialExiste = await _materialRepository.ExistsAsync(id, tallerId);
        if (!materialExiste)
        {
            return null;
        }

        // Validación 2: Si se cambia el nombre, debe ser único por taller
        if (!string.IsNullOrWhiteSpace(request.Nombre))
        {
            var nombreExiste = await _materialRepository.ExistsByNombreAsync(request.Nombre, tallerId, id);
            if (nombreExiste)
            {
                throw new ArgumentException("Ya existe otro material con ese nombre en este taller.");
            }
        }

        // Validación 3: La cantidad no puede ser negativa
        if (request.Cantidad < 0)
        {
            throw new ArgumentException("La cantidad no puede ser negativa.");
        }

        // Si todo está bien, actualiza el material
        return await _materialRepository.UpdateAsync(id, request, tallerId);
    }

    /// <summary>
    /// Elimina un material.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        return await _materialRepository.DeleteAsync(id, tallerId);
    }
}
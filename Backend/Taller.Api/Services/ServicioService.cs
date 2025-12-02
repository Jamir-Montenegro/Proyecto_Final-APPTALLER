// Services/ServicioService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class ServicioService
{
    private readonly IServicioRepository _servicioRepository;
    private readonly IVehiculoRepository _vehiculoRepository;

    // Constructor para inyección de dependencias
    public ServicioService(
        IServicioRepository servicioRepository,
        IVehiculoRepository vehiculoRepository)
    {
        _servicioRepository = servicioRepository;
        _vehiculoRepository = vehiculoRepository;
    }

    /// <summary>
    /// Obtiene todos los servicios del taller especificado.
    /// </summary>
    public async Task<List<ServicioDto>> GetAllAsync(Guid tallerId)
    {
        return await _servicioRepository.GetAllAsync(tallerId);
    }

    /// <summary>
    /// Obtiene un servicio por su ID y taller.
    /// </summary>
    public async Task<ServicioDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        return await _servicioRepository.GetByIdAsync(id, tallerId);
    }

    /// <summary>
    /// Crea un nuevo registro de servicio.
    /// </summary>
    public async Task<ServicioDto> CreateAsync(CreateServicioRequest request, Guid tallerId)
    {
        // Validación 1: El vehículo debe existir y pertenecer al taller
        var vehiculoExiste = await _vehiculoRepository.ExistsAsync(request.VehiculoId, tallerId);
        if (!vehiculoExiste)
        {
            throw new ArgumentException("El vehículo especificado no existe en este taller.");
        }

        // Validación 2: La fecha del servicio no puede estar en el futuro
        if (request.Fecha > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("La fecha del servicio no puede estar en el futuro.");
        }

        // Si todas las validaciones pasan, crea el servicio
        return await _servicioRepository.CreateAsync(request, tallerId);
    }

    /// <summary>
    /// Actualiza un servicio existente.
    /// </summary>
    public async Task<ServicioDto?> UpdateAsync(Guid id, UpdateServicioRequest request, Guid tallerId)
    {
        // Validación 1: Verificar que el servicio exista y pertenezca al taller
        var servicioExiste = await _servicioRepository.ExistsAsync(id, tallerId);
        if (!servicioExiste)
        {
            return null;
        }

        // Validación 2: Si se cambia la fecha, no puede estar en el futuro
        if (request.Fecha.HasValue && request.Fecha.Value > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new ArgumentException("La fecha del servicio no puede estar en el futuro.");
        }

        // Si todo está bien, actualiza el servicio
        return await _servicioRepository.UpdateAsync(id, request, tallerId);
    }

    /// <summary>
    /// Elimina un servicio.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        return await _servicioRepository.DeleteAsync(id, tallerId);
    }
}
// Services/VehiculoService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class VehiculoService
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IClienteRepository _clienteRepository;

    // Constructor para inyección de dependencias
    public VehiculoService(
        IVehiculoRepository vehiculoRepository,
        IClienteRepository clienteRepository)
    {
        _vehiculoRepository = vehiculoRepository;
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Obtiene todos los vehículos del taller especificado.
    /// </summary>
    public async Task<List<VehiculoDto>> GetAllAsync(Guid tallerId)
    {
        return await _vehiculoRepository.GetAllAsync(tallerId);
    }

    /// <summary>
    /// Obtiene un vehículo por su ID y taller.
    /// </summary>
    public async Task<VehiculoDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        return await _vehiculoRepository.GetByIdAsync(id, tallerId);
    }

    /// <summary>
    /// Crea un nuevo vehículo.
    /// </summary>
    public async Task<VehiculoDto> CreateAsync(CreateVehiculoRequest request, Guid tallerId)
    {
        // Validación 1: El cliente debe existir
        var clienteExiste = await _clienteRepository.ExistsAsync(request.ClienteId, tallerId);
        if (!clienteExiste)
        {
            throw new ArgumentException("El cliente especificado no existe.");
        }

        // Validación 2: La placa debe ser única por taller
        var placaExiste = await _vehiculoRepository.ExistsByPlacaAsync(request.Placa, tallerId);
        if (placaExiste)
        {
            throw new ArgumentException("Ya existe un vehículo con esa placa en este taller.");
        }

        // Si todas las validaciones pasan, crea el vehículo
        return await _vehiculoRepository.CreateAsync(request, tallerId);
    }

    /// <summary>
    /// Actualiza un vehículo existente.
    /// </summary>
    public async Task<VehiculoDto?> UpdateAsync(Guid id, UpdateVehiculoRequest request, Guid tallerId)
    {
        // Validación 1: Verificar que el vehículo exista y pertenezca al taller
        var vehiculoExiste = await _vehiculoRepository.ExistsAsync(id, tallerId);
        if (!vehiculoExiste)
        {
            return null;
        }

        // Validación 2: Si se cambia la placa, debe ser única por taller
        if (!string.IsNullOrWhiteSpace(request.Placa)) // Si la placa se está actualizando
        {
            var placaExiste = await _vehiculoRepository.ExistsByPlacaAsync(request.Placa, tallerId, id);
            if (placaExiste)
            {
                throw new ArgumentException("Ya existe otro vehículo con esa placa en este taller.");
            }
        }

        // Si todo está bien, actualiza el vehículo
        return await _vehiculoRepository.UpdateAsync(id, request, tallerId);
    }

    /// <summary>
    /// Elimina un vehículo.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        // Verificar que el vehículo exista y pertenezca al taller (opcional, el repositorio también lo puede hacer)
        return await _vehiculoRepository.DeleteAsync(id, tallerId);
    }
}
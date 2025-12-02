// Services/CitaService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class CitaService
{
    private readonly ICitaRepository _citaRepository;
    private readonly IClienteRepository _clienteRepository;

    // Constructor para inyección de dependencias
    public CitaService(
        ICitaRepository citaRepository,
        IClienteRepository clienteRepository)
    {
        _citaRepository = citaRepository;
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Obtiene todas las citas del taller especificado.
    /// </summary>
    public async Task<List<CitaDto>> GetAllAsync(Guid tallerId)
    {
        return await _citaRepository.GetAllAsync(tallerId);
    }

    /// <summary>
    /// Obtiene una cita por su ID y taller.
    /// </summary>
    public async Task<CitaDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        return await _citaRepository.GetByIdAsync(id, tallerId);
    }

    /// <summary>
    /// Crea una nueva cita.
    /// </summary>
    public async Task<CitaDto> CreateAsync(CreateCitaRequest request, Guid tallerId)
    {
        // Validación 1: El cliente debe existir
        var clienteExiste = await _clienteRepository.ExistsAsync(request.ClienteId, tallerId);
        if (!clienteExiste)
        {
            throw new ArgumentException("El cliente especificado no existe.");
        }

        // Validación 2: La fecha y hora no pueden estar en el pasado
        if (request.FechaHora < DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha y hora de la cita no pueden estar en el pasado.");
        }

        // Si todas las validaciones pasan, crea la cita
        return await _citaRepository.CreateAsync(request, tallerId);
    }

    /// <summary>
    /// Actualiza una cita existente (por ejemplo, cambiar su estado).
    /// </summary>
    public async Task<CitaDto?> UpdateAsync(Guid id, UpdateCitaRequest request, Guid tallerId)
    {
        // Validación 1: Verificar que la cita exista y pertenezca al taller
        var citaExiste = await _citaRepository.ExistsAsync(id, tallerId);
        if (!citaExiste)
        {
            return null;
        }

        // Validación 2: Si se cambia la fecha, no puede estar en el pasado
        if (request.FechaHora.HasValue && request.FechaHora.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha y hora de la cita no pueden estar en el pasado.");
        }

        // Si todo está bien, actualiza la cita
        return await _citaRepository.UpdateAsync(id, request, tallerId);
    }

    /// <summary>
    /// Elimina una cita.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        return await _citaRepository.DeleteAsync(id, tallerId);
    }
}
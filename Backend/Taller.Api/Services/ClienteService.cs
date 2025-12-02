// Services/ClienteService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class ClienteService
{
    private readonly IClienteRepository _clienteRepository;

    // Constructor para inyección de dependencias
    public ClienteService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    /// <summary>
    /// Obtiene todos los clientes del taller especificado.
    /// </summary>
    public async Task<List<ClienteDto>> GetAllAsync(Guid tallerId)
    {
        return await _clienteRepository.GetAllAsync(tallerId);
    }

    /// <summary>
    /// Obtiene un cliente por su ID y taller.
    /// </summary>
    public async Task<ClienteDto?> GetByIdAsync(Guid id, Guid tallerId)
    {
        return await _clienteRepository.GetByIdAsync(id, tallerId);
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    public async Task<ClienteDto> CreateAsync(CreateClienteRequest request, Guid tallerId)
    {
        // Validación 1: La cédula debe ser única por taller
        if (!string.IsNullOrWhiteSpace(request.Cedula))
        {
            var cedulaExiste = await _clienteRepository.ExistsByCedulaAsync(request.Cedula, tallerId);
            if (cedulaExiste)
            {
                throw new ArgumentException("Ya existe un cliente con esa cédula en este taller.");
            }
        }

        // Si la validación pasa, crea el cliente
        return await _clienteRepository.CreateAsync(request, tallerId);
    }

    /// <summary>
    /// Actualiza un cliente existente.
    /// </summary>
    public async Task<ClienteDto?> UpdateAsync(Guid id, UpdateClienteRequest request, Guid tallerId)
    {
        // Validación 1: Verificar que el cliente exista y pertenezca al taller
        var clienteExiste = await _clienteRepository.ExistsAsync(id, tallerId);
        if (!clienteExiste)
        {
            return null;
        }

        // Validación 2: Si se cambia la cédula, debe ser única por taller
        if (!string.IsNullOrWhiteSpace(request.Cedula))
        {
            var cedulaExiste = await _clienteRepository.ExistsByCedulaAsync(request.Cedula, tallerId, id);
            if (cedulaExiste)
            {
                throw new ArgumentException("Ya existe otro cliente con esa cédula en este taller.");
            }
        }

        // Si todo está bien, actualiza el cliente
        return await _clienteRepository.UpdateAsync(id, request, tallerId);
    }

    /// <summary>
    /// Elimina un cliente.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id, Guid tallerId)
    {
        // Verificar que el cliente exista y pertenezca al taller (opcional, el repositorio también lo puede hacer)
        return await _clienteRepository.DeleteAsync(id, tallerId);
    }
}
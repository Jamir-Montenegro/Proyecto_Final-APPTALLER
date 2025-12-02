// Services/InformeService.cs
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class InformeService
{
    private readonly IVehiculoRepository _vehiculoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly ICitaRepository _citaRepository;

    // Constructor para inyección de dependencias
    public InformeService(
        IVehiculoRepository vehiculoRepository,
        IClienteRepository clienteRepository,
        ICitaRepository citaRepository)
    {
        _vehiculoRepository = vehiculoRepository;
        _clienteRepository = clienteRepository;
        _citaRepository = citaRepository;
    }

    /// <summary>
    /// Obtiene el informe general del taller (KPIs y estadísticas).
    /// </summary>
    public async Task<InformeDto> GetInformeAsync(Guid tallerId)
    {
        // 1. Obtener el total de vehículos
        var totalVehiculos = await _vehiculoRepository.GetCountAsync(tallerId);

        // 2. Obtener el total de clientes
        var totalClientes = await _clienteRepository.GetCountAsync(tallerId);

        // 3. Obtener el total de citas solicitadas
        var totalCitas = await _citaRepository.GetCountAsync(tallerId);

        // 4. Obtener el total de citas por estado
        var citasAtendidas = await _citaRepository.GetCountByEstadoAsync("Atendida", tallerId);
        var citasPendientes = await _citaRepository.GetCountByEstadoAsync("Pendiente", tallerId);
        var citasCanceladas = await _citaRepository.GetCountByEstadoAsync("Cancelada", tallerId);

        // 5. Calcular la tasa de citas completadas
        var tasaCompletadas = totalCitas > 0 
            ? Math.Round((double)citasAtendidas / totalCitas * 100, 2) 
            : 0;

        // 6. Devolver el informe
        return new InformeDto
        {
            VehiculosRegistrados = totalVehiculos,
            ClientesRegistrados = totalClientes,
            CitasSolicitadas = totalCitas,
            CitasAtendidas = citasAtendidas,
            CitasPendientes = citasPendientes,
            CitasCanceladas = citasCanceladas,
            TasaCitasCompletadas = tasaCompletadas
        };
    }
}
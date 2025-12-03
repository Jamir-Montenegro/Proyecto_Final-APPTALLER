using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.Api.Models;
using Taller.Api.Services;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //  Ahora requiere JWT
public class VehiculosController : ControllerBase
{
    private readonly VehiculoService _vehiculoService;

    public VehiculosController(VehiculoService vehiculoService)
    {
        _vehiculoService = vehiculoService;
    }

    [HttpGet]
    public async Task<IActionResult> GetVehiculos()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var vehiculos = await _vehiculoService.GetAllAsync(tallerId);
            return Ok(vehiculos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al obtener los vehículos.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetVehiculo(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var vehiculo = await _vehiculoService.GetByIdAsync(id, tallerId);

            if (vehiculo == null)
                return NotFound(new { message = "Vehículo no encontrado." });

            return Ok(vehiculo);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al obtener el vehículo.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehiculo([FromBody] CreateVehiculoRequest request)
    {
        try
        {

        if (request.Anio < 1950 || request.Anio > DateTime.Now.Year)
    return BadRequest(new { message = "El año del vehículo no es válido." });

       if (string.IsNullOrWhiteSpace(request.Placa))
    return BadRequest(new { message = "La placa es obligatoria." });

       if (!string.IsNullOrEmpty(request.Vin) && request.Vin.Length != 17)
    return BadRequest(new { message = "El VIN debe tener 17 caracteres." });

            var tallerId = ObtenerTallerIdDelToken();
            var vehiculo = await _vehiculoService.CreateAsync(request, tallerId);

            return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.Id }, vehiculo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al crear el vehículo.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVehiculo(Guid id, [FromBody] UpdateVehiculoRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var vehiculo = await _vehiculoService.UpdateAsync(id, request, tallerId);

            if (vehiculo == null)
                return NotFound(new { message = "Vehículo no encontrado." });

            return Ok(vehiculo);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al actualizar el vehículo.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehiculo(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var deleted = await _vehiculoService.DeleteAsync(id, tallerId);

            if (!deleted)
                return NotFound(new { message = "Vehículo no encontrado." });

            return Ok(new { message = "Vehículo eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al eliminar el vehículo.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //  Obtener el ID real del taller desde el JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene tallerId.");

        return Guid.Parse(claim);
    }
}


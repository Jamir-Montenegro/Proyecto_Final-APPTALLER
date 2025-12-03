using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.Api.Models;
using Taller.Api.Services;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  //  Ahora requiere JWT
public class ServiciosController : ControllerBase
{
    private readonly ServicioService _servicioService;

    public ServiciosController(ServicioService servicioService)
    {
        _servicioService = servicioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetServicios()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var servicios = await _servicioService.GetAllAsync(tallerId);
            return Ok(servicios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Error al obtener los servicios.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetServicio(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var servicio = await _servicioService.GetByIdAsync(id, tallerId);

            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado." });

            return Ok(servicio);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Error al obtener el servicio.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateServicio([FromBody] CreateServicioRequest request)
    {
        try
        {

        if (request.Costo < 0)
    return BadRequest(new { message = "El costo no puede ser negativo." });

        if (string.IsNullOrWhiteSpace(request.Mecanico))
    return BadRequest(new { message = "Debe especificar el mecánico." });

            var tallerId = ObtenerTallerIdDelToken();
            var servicio = await _servicioService.CreateAsync(request, tallerId);

            return CreatedAtAction(nameof(GetServicio), new { id = servicio.Id }, servicio);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            {
                message = "Error al crear el servicio.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateServicio(Guid id, [FromBody] UpdateServicioRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var servicio = await _servicioService.UpdateAsync(id, request, tallerId);

            if (servicio == null)
                return NotFound(new { message = "Servicio no encontrado." });

            return Ok(servicio);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            {
                message = "Error al actualizar el servicio.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteServicio(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var deleted = await _servicioService.DeleteAsync(id, tallerId);

            if (!deleted)
                return NotFound(new { message = "Servicio no encontrado." });

            return Ok(new { message = "Servicio eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            {
                message = "Error al eliminar el servicio.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //  el tallerId real desde el token JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrWhiteSpace(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene tallerId.");

        return Guid.Parse(claim);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taller.Api.Models;
using Taller.Api.Services;
using System.Security.Claims;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // controlador requiere token JWT
public class CitasController : ControllerBase
{
    private readonly CitaService _citaService;

    public CitasController(CitaService citaService)
    {
        _citaService = citaService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCitas()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var citas = await _citaService.GetAllAsync(tallerId);
            return Ok(citas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al obtener las citas.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCita(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cita = await _citaService.GetByIdAsync(id, tallerId);

            if (cita == null)
                return NotFound(new { message = "Cita no encontrada." });

            return Ok(cita);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al obtener la cita.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCita([FromBody] CreateCitaRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cita = await _citaService.CreateAsync(request, tallerId);

            return CreatedAtAction(nameof(GetCita), new { id = cita.Id }, cita);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                error = ex.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al crear la cita.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCita(Guid id, [FromBody] UpdateCitaRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cita = await _citaService.UpdateAsync(id, request, tallerId);

            if (cita == null)
                return NotFound(new { message = "Cita no encontrada." });

            return Ok(cita);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new
            {
                message = ex.Message,
                error = ex.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al actualizar la cita.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCita(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var deleted = await _citaService.DeleteAsync(id, tallerId);

            if (!deleted)
                return NotFound(new { message = "Cita no encontrada." });

            return Ok(new { message = "Cita eliminada correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Error al eliminar la cita.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //obtiene el tallerId REAL desde el JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene el tallerId.");

        return Guid.Parse(claim);
    }
}

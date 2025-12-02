using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taller.Api.Models;
using Taller.Api.Services;
using System.Security.Claims;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //  este controlador requiere token válido
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetClientes()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var clientes = await _clienteService.GetAllAsync(tallerId);
            return Ok(clientes);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al obtener los clientes.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCliente(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cliente = await _clienteService.GetByIdAsync(id, tallerId);

            if (cliente == null)
                return NotFound(new { message = "Cliente no encontrado." });

            return Ok(cliente);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al obtener el cliente.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateCliente([FromBody] CreateClienteRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cliente = await _clienteService.CreateAsync(request, tallerId);

            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {
                message = ex.Message,
                error = ex.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al crear el cliente.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCliente(Guid id, [FromBody] UpdateClienteRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var cliente = await _clienteService.UpdateAsync(id, request, tallerId);

            if (cliente == null)
                return NotFound(new { message = "Cliente no encontrado." });

            return Ok(cliente);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new {
                message = ex.Message,
                error = ex.ToString()
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al actualizar el cliente.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCliente(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var deleted = await _clienteService.DeleteAsync(id, tallerId);

            if (!deleted)
                return NotFound(new { message = "Cliente no encontrado." });

            return Ok(new { message = "Cliente eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al eliminar el cliente.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //  Extrae el tallerId real desde el token JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrEmpty(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene tallerId.");

        return Guid.Parse(claim);
    }
}

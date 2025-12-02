using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.Api.Models;
using Taller.Api.Services;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // requiere JWT
public class MaterialesController : ControllerBase
{
    private readonly MaterialService _materialService;

    public MaterialesController(MaterialService materialService)
    {
        _materialService = materialService;
    }

    [HttpGet]
    public async Task<IActionResult> GetMateriales()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var materiales = await _materialService.GetAllAsync(tallerId);
            return Ok(materiales);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al obtener los materiales.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaterial(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var material = await _materialService.GetByIdAsync(id, tallerId);

            if (material == null)
                return NotFound(new { message = "Material no encontrado." });

            return Ok(material);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al obtener el material.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var material = await _materialService.CreateAsync(request, tallerId);
            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
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
                message = "Error al crear el material.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMaterial(Guid id, [FromBody] UpdateMaterialRequest request)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var material = await _materialService.UpdateAsync(id, request, tallerId);

            if (material == null)
                return NotFound(new { message = "Material no encontrado." });

            return Ok(material);
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
                message = "Error al actualizar el material.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMaterial(Guid id)
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var deleted = await _materialService.DeleteAsync(id, tallerId);

            if (!deleted)
                return NotFound(new { message = "Material no encontrado." });

            return Ok(new { message = "Material eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new {
                message = "Error al eliminar el material.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //  el tallerId correcto del JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrWhiteSpace(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene tallerId.");

        return Guid.Parse(claim);
    }
}

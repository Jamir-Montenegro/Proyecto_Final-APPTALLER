using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Taller.Api.Models;
using Taller.Api.Services;

namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] //  requiere token JWT válido
public class InformeController : ControllerBase
{
    private readonly InformeService _informeService;

    public InformeController(InformeService informeService)
    {
        _informeService = informeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInforme()
    {
        try
        {
            var tallerId = ObtenerTallerIdDelToken();
            var informe = await _informeService.GetInformeAsync(tallerId);
            return Ok(informe);
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
                message = "Error al generar el informe.",
                error = ex.Message,
                stack = ex.ToString()
            });
        }
    }

    //  el tallerId real contenido en el JWT
    private Guid ObtenerTallerIdDelToken()
    {
        var claim = User.Claims.FirstOrDefault(c => c.Type == "tallerId")?.Value;

        if (string.IsNullOrWhiteSpace(claim))
            throw new UnauthorizedAccessException("Token inválido: no contiene tallerId.");

        return Guid.Parse(claim);
    }
}

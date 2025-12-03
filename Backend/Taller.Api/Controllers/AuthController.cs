// Controllers/AuthController.cs
using Microsoft.AspNetCore.Mvc;
using Taller.Api.Models;
using Taller.Api.Services;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
namespace Taller.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    /// <summary>
    /// Constructor del controlador de autenticación.
    /// </summary>
    /// <param name="authService">Servicio de autenticación inyectado.</param>
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Endpoint para iniciar sesión en el sistema del taller.
    /// </summary>
    /// <param name="request">Los datos de login (email y contraseña).</param>
    /// <returns>Los datos del taller y un token si las credenciales son válidas.</returns>
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var user = await _authService.LoginAsync(request);
            if (user == null)
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch
        {
            return StatusCode(500, new { message = "Error interno del servidor." });
        }
    }

    /// <summary>
    /// Endpoint para registrar un nuevo taller.
    /// </summary>
    /// <param name="request">Los datos del nuevo taller (nombre, email, contraseña).</param>
    /// <returns>Los datos del nuevo taller y un token.</returns
    /// 
    
    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {

         if (string.IsNullOrWhiteSpace(request.Nombre))
            return BadRequest(new { message = "El nombre del taller es obligatorio." });

        if (string.IsNullOrWhiteSpace(request.Email))
            return BadRequest(new { message = "El email es obligatorio." });

         var emailValidator = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (!emailValidator.IsValid(request.Email))
            return BadRequest(new { message = "El formato del email no es válido." });

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new { message = "La contraseña es obligatoria." });

        if (request.Password.Length < 8)
            return BadRequest(new { message = "La contraseña debe tener al menos 8 caracteres." });

        
        bool mayus = request.Password.Any(char.IsUpper);
        bool minus = request.Password.Any(char.IsLower);
        bool num = request.Password.Any(char.IsDigit);

        if (!mayus || !minus || !num)
        {
            return BadRequest(new
            {
                message = "La contraseña debe contener mayúsculas, minúsculas y números."
            });
        }

       

            if (request.Password != request.ConfirmPassword)
            {
                return BadRequest(new { message = "Las contraseñas no coinciden." });
            }

            var user = await _authService.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest(new { message = "El email ya está en uso." });
            }
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch
        {
            return StatusCode(500, new { message = "Error interno del servidor." });
        }
    }
}

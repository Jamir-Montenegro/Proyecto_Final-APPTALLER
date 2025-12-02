// Services/AuthService.cs
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Taller.Api.Data.Repositories;
using Taller.Api.Models;

namespace Taller.Api.Services;

public class AuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly IConfiguration _config;

    // Constructor para inyección de dependencias
    public AuthService(IAuthRepository authRepository, IConfiguration config)
    {
        _authRepository = authRepository;
        _config = config;
    }

    /// <summary>
    /// Intenta iniciar sesión con las credenciales proporcionadas.
    /// </summary>
    /// <param name="request">Los datos de login (email y contraseña).</param>
    /// <returns>Los datos del taller y un token si las credenciales son válidas; null en caso contrario.</returns>
    public async Task<UserResponse?> LoginAsync(LoginRequest request)
    {
        // Valida los datos de entrada (opcional, pero bueno)
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            return null;
        }

        // Delega la operación de base de datos al repositorio
        var user = await _authRepository.LoginAsync(request);

        Console.WriteLine(">>> LOGIN RECIBIDO Password=" + request.Password);


        if (user == null)
            return null;

        //  Generar el token JWT aquí
        user.Token = GenerateJwt(user.Id, user.Email, user.Nombre);

        return user;
    }

    /// <summary>
    /// Registra un nuevo taller.
    /// </summary>
    /// <param name="request">Los datos del nuevo taller.</param>
    /// <returns>Los datos del nuevo taller y un token; null si el email ya existe.</returns>
    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        // Valida los datos de entrada
        if (string.IsNullOrWhiteSpace(request.Nombre) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            request.Password != request.ConfirmPassword)
        {
            return null;
        }

        // Delega la operación de base de datos al repositorio
        var user = await _authRepository.RegisterAsync(request);

        Console.WriteLine(">>> REGISTER RECIBIDO:");
        Console.WriteLine("Nombre: " + request.Nombre);
        Console.WriteLine("Email: " + request.Email);
        Console.WriteLine("Password: " + request.Password) ;
        Console.WriteLine("Confirm: " + request.ConfirmPassword);


        if (user == null)
            return null;

        // Generar token JWT para el nuevo usuario
        user.Token = GenerateJwt(user.Id, user.Email, user.Nombre);

        return user;
    }

    /// <summary>
    /// Genera un token JWT válido para el usuario.
    /// </summary>
    /// <param name="tallerId">Id del taller autenticado.</param>
    /// <param name="email">Email del usuario.</param>
    /// <param name="nombre">Nombre del usuario.</param>
    /// <returns>Un string con el token JWT.</returns>
    private string GenerateJwt(Guid tallerId, string email, string nombre)
    {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("tallerId", tallerId.ToString()),
            new Claim("email", email),
            new Claim("nombre", nombre),
            new Claim(JwtRegisteredClaimNames.Sub, email)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

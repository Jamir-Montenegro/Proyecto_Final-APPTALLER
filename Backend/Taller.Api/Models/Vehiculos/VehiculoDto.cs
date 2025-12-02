
namespace Taller.Api.Models;

/// <summary>
/// Representa un vehículo en el sistema.
/// Incluye información del vehículo y del cliente propietario.
/// </summary>
public class VehiculoDto
{
    /// <summary>
    /// Obtiene o establece el identificador único del vehículo.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Obtiene o establece el identificador del cliente propietario.
    /// </summary>
    public Guid ClienteId { get; set; }

    /// <summary>
    /// Obtiene o establece el nombre del cliente propietario.
    /// Se incluye para facilitar la visualización en el frontend.
    /// </summary>
    /// <example>Juan Pérez</example>
    public string ClienteNombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el número de cédula del cliente propietario.
    /// Se incluye para diferenciar clientes con el mismo nombre.
    /// </summary>
    /// <example>123-456-7890-X</example>
    public string ClienteCedula { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la marca del vehículo.
    /// </summary>
    /// <example>Toyota</example>
    public string Marca { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el modelo del vehículo.
    /// </summary>
    /// <example>Corolla</example>
    public string Modelo { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el año de fabricación del vehículo.
    /// </summary>
    /// <example>2020</example>
    public int Anio { get; set; }

    /// <summary>
    /// Obtiene o establece la placa del vehículo.
    /// Debe ser única por taller.
    /// </summary>
    /// <example>ABC-123</example>
    public string Placa { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el color del vehículo.
    /// </summary>
    /// <example>Blanco</example>
    public string Color { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el número de chasis (VIN) del vehículo.
    /// </summary>
    /// <example>1HGBH41JXMN109186</example>
    public string Vin { get; set; } = string.Empty;
}
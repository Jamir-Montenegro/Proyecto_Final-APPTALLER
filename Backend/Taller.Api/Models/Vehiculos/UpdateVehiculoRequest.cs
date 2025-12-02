
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud para actualizar un vehículo existente.
/// Contiene los datos que se pueden modificar de un vehículo ya registrado.
/// </summary>
public class UpdateVehiculoRequest
{
    /// <summary>
    /// Obtiene o establece el identificador del cliente propietario del vehículo.
    /// Este cliente debe existir previamente en el sistema.
    /// </summary>
    public Guid ClienteId { get; set; }

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
    /// Debe ser única dentro del mismo taller.
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
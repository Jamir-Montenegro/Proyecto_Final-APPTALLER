
namespace Taller.Api.Models;

/// <summary>
/// Representa la solicitud para registrar un nuevo material en el inventario.
/// Contiene los datos necesarios para crear un registro de material.
/// </summary>
public class CreateMaterialRequest
{
    /// <summary>
    /// Obtiene o establece el nombre del material.
    /// Debe ser único dentro del mismo taller.
    /// </summary>
    /// <example>Aceite de motor 5W-30</example>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la descripción del material.
    /// </summary>
    /// <example>Aceite sintético para motores a gasolina</example>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece la cantidad inicial en stock.
    /// </summary>
    /// <example>25</example>
    public int Cantidad { get; set; } = 0;

    /// <summary>
    /// Obtiene o establece el umbral de stock bajo.
    /// Cuando la cantidad es menor o igual a este valor, se activa una alerta.
    /// </summary>
    /// <example>5</example>
    public int UmbralBajo { get; set; } = 5;

    /// <summary>
    /// Obtiene o establece el precio unitario del material.
    /// </summary>
    /// <example>25.99</example>
    public decimal PrecioUnitario { get; set; } = 0m;

    /// <summary>
    /// Obtiene o establece la categoría del material.
    /// </summary>
    /// <example>Lubricantes</example>
    public string Categoria { get; set; } = string.Empty;

    /// <summary>
    /// Obtiene o establece el nombre del proveedor del material.
    /// </summary>
    /// <example>Distribuidora ABC S.A.</example>
    public string Proveedor { get; set; } = string.Empty;
}
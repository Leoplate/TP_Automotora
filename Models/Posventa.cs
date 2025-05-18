namespace technical_tests_backend_ssr.Models;

/// <summary>
/// Producto refleja la esencia de la boutique: artefactos exclusivos para autos.
/// </summary>
public class Posventa
{
    /// <summary>
    /// Identificador único del producto.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;
    /// <summary>
    /// Precio del producto.
    /// </summary>
    public int Tipoid { get; set; }
    public Tipo Tipo { get; set; } = null!;
    /// <summary>
    /// sotck del producto.
    /// </summary>
    public DateOnly Fecha { get; set; }

    public int Estadoid { get; set; }
    public Estado Estado { get; set; } = null!;
}
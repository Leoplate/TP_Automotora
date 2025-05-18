
/// <summary>
/// Producto refleja la esencia de la boutique: artefactos exclusivos para autos.
/// </summary>
public class VentaCompletaDTO
{
    // Datos de la venta
    public int Id { get; set; }
    public DateOnly Fecha { get; set; }
    public int Total { get; set; }

    // Datos del cliente relacionado
    public int ClienteId { get; set; }
    public string ClienteNombre { get; set; }

    public string ClienteApellido { get; set; }
    public string ClienteEmail { get; set; }
    public string ClienteTelefono { get; set; }

    // Datos del producto relacionado
    public int ProductoId { get; set; }
    public string ProductoNombre { get; set; }
    public decimal ProductoPrecio { get; set; }
    //public int ProductoStock { get; set; }
}


/// <summary>
/// Producto refleja la esencia de la boutique: artefactos exclusivos para autos.
/// </summary>
public class PosventaCompletaDTO
{
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public int ClienteId { get; set; }

    public string ClienteNombre { get; set; }

    public string ClienteApellido { get; set; }
    /// <summary>
    /// Precio del producto.    
    /// </summary>

    public DateOnly Fecha { get; set; }
    /// <summary>
    /// Precio del producto.
    /// </summary>


    public int Tipoid { get; set; }

    public string TipoDescripcion { get; set; }

    public int Estadoid { get; set; }

    public string EstadoDescripcion { get; set; }

}


/// <summary>
/// Producto refleja la esencia de la boutique: artefactos exclusivos para autos.
/// </summary>
public class PosventaDTO
{
    public int Id { get; set; }

    /// <summary>
    /// Nombre del producto.
    /// </summary>
    public int ClienteId { get; set; }

    /// <summary>
    /// Precio del producto.    
    /// </summary>

    //public int VehiculoId { get; set; }


    public DateOnly Fecha { get; set; }
    /// <summary>
    /// Precio del producto.
    /// </summary>


    public int Tipoid { get; set; }

    public int Estadoid { get; set; }
 
}

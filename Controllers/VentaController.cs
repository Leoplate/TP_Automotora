using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class VentaController : ControllerBase
{
    private readonly VentaService _ventaService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ventaService"></param>
    /// <param name="mapper"></param>
    public VentaController(VentaService ventaService, IMapper mapper)
    {
        _ventaService = ventaService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VentaCompletaDTO>>> GetAll()
    {
        var ventas = await _ventaService.GetAllVentasAsync();
        return Ok(_mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<VentaCompletaDTO>> GetById(int id)
    {
        var venta = await _ventaService.GetVentaByIdAsync(id);
        if (venta == null) return NotFound();
        
        return Ok(_mapper.Map<VentaCompletaDTO>(venta));
        
        

    }


    /// <summary>
    /// Agregar un nuevo producto    
    /// </summary>
    /// <param name="ventaDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<VentaDTO>> Create(VentaDTO ventaDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var venta = _mapper.Map<Venta>(ventaDTO);
        //var (newVenta,error) = await _ventaService.AddVentaAsync(venta);
       var newVenta = await _ventaService.AddVentaAsync(venta);

        if (newVenta !=null) { 
        return CreatedAtAction(nameof(GetById), new { id = newVenta.Id }, _mapper.Map<VentaDTO>(newVenta));
        }
            return BadRequest("No hay stock del producto");
            
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VentaDTO>> Update(int id, [FromBody] VentaDTO ventaDTO)
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}

        var venta = await _ventaService.GetVentaByIdAsync(id);
        if (venta == null)
        {
            return NotFound($"No se encontró el cliente con ID {id}.");
        }

        _mapper.Map(ventaDTO, venta);
        await _ventaService.UpdateVentaAsync(venta);

        var updatedVentaDTO = _mapper.Map<VentaDTO>(venta);
        return Ok(updatedVentaDTO); // Retornar el producto actualizado
    }

    /// <summary>
    /// Eliminar un producto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _ventaService.DeleteVentaAsync(id);
        if (!deleted) return NotFound($"No se encontró la venta ID {id}.");
        return Ok($"Se eliminó la venta ID {id}.");
    }

    [HttpPost("random")]
    public async Task  UpdateRandom()
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}




         await _ventaService.AddPosventaAsyncRandom();


    }


    [HttpGet("vehiculo")]
    public async Task<ActionResult<IEnumerable<TopVehiculoDTO>>> GetListVehiculo()
    {
        var ventas = await _ventaService.GetAllVentasAsync();




        var vehiculos = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas)
       .GroupBy(v => v.ProductoNombre)
       .Select(g => new
       {
           VehiculoId = g.Key,
           TotalVentas = g.Count()
       })
       .OrderByDescending(e => e.TotalVentas)
       .Take(5) 
       .ToList();

        

        List<TopVehiculoDTO> top = new List<TopVehiculoDTO>();

        foreach (var vehiculo in vehiculos)
        {
            var topVehiculo = new TopVehiculoDTO
            {
                Vehiculo = vehiculo.VehiculoId,
                Total = vehiculo.TotalVentas,
            };
            top.Add(topVehiculo);
        }

        return Ok(top);
    }

    [HttpGet("cliente")]
    public async Task<ActionResult<IEnumerable<TopClienteDTO>>> GetListCliente()
    {
        var ventas = await _ventaService.GetAllVentasAsync();

        var clientes = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas)
    .GroupBy(v => new { v.ClienteId, v.ClienteNombre, v.ClienteApellido })
    .Select(g => new
    {
        ClienteId = g.Key,
        Nombre = g.Key.ClienteNombre + " " + g.Key.ClienteApellido,
        TotalVentas = g.Count()
    })
    .OrderByDescending(e => e.TotalVentas)
    .Take(5) 
    .ToList();


        

        

        List<TopClienteDTO> top = new List<TopClienteDTO>();

        foreach (var cliente in clientes)
        {
            var topcliente = new TopClienteDTO
            {
                Nombre = cliente.Nombre,
                Total = cliente.TotalVentas,
            };
            top.Add(topcliente);
        }

        return Ok(top);
    }

}

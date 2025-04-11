using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


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
        Console.WriteLine("HOLA");
        var venta = _mapper.Map<Venta>(ventaDTO);
        var newVenta = await _ventaService.AddVentaAsync(venta);
        return CreatedAtAction(nameof(GetById), new { id = newVenta.Id }, _mapper.Map<VentaDTO>(newVenta));
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
        await _ventaService.UpdateClientAsync(venta);

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
        if (!deleted) return NotFound();
        return NoContent();
    }
}

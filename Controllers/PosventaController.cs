using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class PosventaController : ControllerBase
{
    private readonly PosventaService _posventaService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="posventaService"></param>
    /// <param name="mapper"></param>
    public PosventaController(PosventaService posventaService, IMapper mapper)
    {
        _posventaService = posventaService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PosventaCompletaDTO>>> GetAll()
    {
        var posventas = await _posventaService.GetAllPostventasAsync();
        return Ok(_mapper.Map<IEnumerable<PosventaCompletaDTO>>(posventas));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PosventaCompletaDTO>> GetById(int id)
    {
        var posventa = await _posventaService.GetPosventaByIdAsync(id);
        if (posventa == null) return NotFound($"No se encontró el ID {id}.");
        return Ok(_mapper.Map<PosventaCompletaDTO>(posventa));
        //return Ok(posventa);
    }


    /// <summary>
    /// Agregar un nuevo producto    
    /// </summary>
    /// <param name="posventaDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<PosventaDTO>> Create(PosventaDTO posventaDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var posventa = _mapper.Map<Posventa>(posventaDTO);
        var newVenta = await _posventaService.AddPosventaAsync(posventa);
       
        
        if (newVenta !=null) { 
        return CreatedAtAction(nameof(GetById), new { id = newVenta.Id }, _mapper.Map<PosventaDTO>(newVenta));
        }
            return NotFound("Posventa no realizada");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<PosventaDTO>> Update(int id, [FromBody] PosventaDTO posventaDTO)
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}

        var posventa = await _posventaService.GetPosventaByIdAsync(id);
        if (posventa == null)
        {
            return NotFound($"No se encontró el cliente con ID {id}.");
        }

        _mapper.Map(posventaDTO, posventa);
        await _posventaService.UpdatePosventaAsync(posventa);

        var updatedPosventaDTO = _mapper.Map<PosventaDTO>(posventa);
        return Ok(updatedPosventaDTO); // Retornar el producto actualizado
    }

    /// <summary>
    /// Eliminar un producto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _posventaService.DeletePosventaAsync(id);
        if (!deleted) return NotFound($"No se encontró la posventa ID {id}.");
        return NotFound($"Se eliminó la posventa ID {id}.");
    }



    [HttpPost("posrandom")]
    public async Task UpdateRandom()
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}




        await _posventaService.AddPosventaAsyncRandom();


    }





}

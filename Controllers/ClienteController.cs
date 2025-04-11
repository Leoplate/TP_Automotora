using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClienteController : ControllerBase
{
    private readonly ClientService _clientService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientService"></param>
    /// <param name="mapper"></param>
    public ClienteController(ClientService clientService, IMapper mapper)
    {
        _clientService = clientService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClienteDTO>>> GetAll()
    {
        var clients = await _clientService.GetAllClientsAsync();
        return Ok(_mapper.Map<IEnumerable<ClienteDTO>>(clients));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClienteDTO>> GetById(int id)
    {
        var client = await _clientService.GetClientByIdAsync(id);
        if (client == null) return NotFound();
        return Ok(_mapper.Map<ClienteDTO>(client));
    }


    /// <summary>
    /// Agregar un nuevo producto    
    /// </summary>
    /// <param name="clienteDTO"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ClienteDTO>> Create(ClienteDTO clienteDTO)
    {
        // FluentValidation se hace automáticamente al verificar ModelState.IsValid.
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var cliente = _mapper.Map<Cliente>(clienteDTO);
        var newCliente = await _clientService.AddClientAsync(cliente);
        return CreatedAtAction(nameof(GetById), new { id = newCliente.Id }, _mapper.Map<ClienteDTO>(newCliente));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ClienteDTO>> Update(int id, [FromBody] ClienteDTO clienteDTO)
    {
        //if (id != productoDTO.Id)
        //{
        //    return BadRequest("El ID del producto no coincide con el de la URL.");
        //}

        var cliente = await _clientService.GetClientByIdAsync(id);
        if (cliente == null)
        {
            return NotFound($"No se encontró el cliente con ID {id}.");
        }

        _mapper.Map(clienteDTO, cliente);
        await _clientService.UpdateClientAsync(cliente);

        var updatedClienteDTO = _mapper.Map<ClienteDTO>(cliente);
        return Ok(updatedClienteDTO); // Retornar el producto actualizado
    }

    /// <summary>
    /// Eliminar un producto.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _clientService.DeleteClientAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}

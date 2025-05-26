using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class EstadoController : ControllerBase
{
    private readonly EstadoService _estadoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="estadoService"></param>
    /// <param name="mapper"></param>
    public EstadoController(EstadoService estadoService, IMapper mapper)
    {
        _estadoService = estadoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estado>>> GetAll()
    {
        var estados = await _estadoService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<Estado>>(estados));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Estado>> GetById(int id)
    {
        var estado = await _estadoService.GetByIdAsync(id);
        if (estado == null) return NotFound();
        return Ok(_mapper.Map<Estado>(estado));
    }


    
    
}

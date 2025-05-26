using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using technical_tests_backend_ssr.Models;


/// <summary>
/// 
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TipoController : ControllerBase
{
    private readonly TipoService _tipoService;
    private readonly IMapper _mapper;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tipoService"></param>
    /// <param name="mapper"></param>
    public TipoController(TipoService tipoService, IMapper mapper)
    {
        _tipoService = tipoService;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtener todos los productos.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tipo>>> GetAll()
    {
        var tipos = await _tipoService.GetAllTiposAsync();
        return Ok(_mapper.Map<IEnumerable<Tipo>>(tipos));
    }

    /// <summary>
    /// Obtener un producto por su ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Tipo>> GetById(int id)
    {
        var tipo = await _tipoService.GetTipoByIdAsync(id);
        if (tipo == null) return NotFound();
        return Ok(_mapper.Map<Tipo>(tipo));
    }


    
    
}

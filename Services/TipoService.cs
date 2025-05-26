using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Threading;


public class TipoService
{
    private readonly ITipoRepository _tipoRepository;
    


    public TipoService(ITipoRepository tipoRepository)
    {
        _tipoRepository = tipoRepository;
    }

    public Task<IEnumerable<Tipo>> GetAllTiposAsync()
    {
        return _tipoRepository.GetAllAsync();
    }

    public Task<Tipo?> GetTipoByIdAsync(int id)
    {
        return _tipoRepository.GetByIdAsync(id);
    }

    
}

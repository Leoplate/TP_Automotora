using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Threading;


public class EstadoService
{
    private readonly IEstadoRepository _estadoRepository;
    


    public EstadoService(IEstadoRepository estadoRepository)
    {
        _estadoRepository = estadoRepository;
    }

    public Task<IEnumerable<Estado>> GetAllAsync()
    {
        return _estadoRepository.GetAllAsync();
    }

    public Task<Estado?> GetByIdAsync(int id)
    {
        return _estadoRepository.GetByIdAsync(id);
    }

    
}

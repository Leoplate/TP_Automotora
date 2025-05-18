using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
public interface IEstadoRepository
{
    Task<IEnumerable<Estado>> GetAllAsync();
    Task<Estado?> GetByIdAsync(int id);
    //Task AddAsync(Cliente cliente);
    //Task UpdateAsync(Cliente cliente);
    //Task DeleteAsync(int id);
}

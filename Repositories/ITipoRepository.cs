using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
public interface ITipoRepository
{
    Task<IEnumerable<Tipo>> GetAllAsync();
    Task<Tipo?> GetByIdAsync(int id);
    //Task AddAsync(Cliente cliente);
    //Task UpdateAsync(Cliente cliente);
    //Task DeleteAsync(int id);
}

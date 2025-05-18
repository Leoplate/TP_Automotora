using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories;
public interface IPosventaRepository
{
    Task<IEnumerable<Posventa>> GetAllAsync();
    Task<Posventa?> GetByIdAsync(int id);
    Task AddAsync(Posventa venta);
    Task UpdateAsync(Posventa venta);
    Task DeleteAsync(int id);

    Task AddAsyncRamdom(Posventa venta);
}

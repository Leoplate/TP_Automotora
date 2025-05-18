using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

public class EstadoRepository : IEstadoRepository
{
    private readonly AppDbContext _context;

    public EstadoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Estado>> GetAllAsync()
    {
        return await _context.Estado
            //.Include(v => v.Cliente) 
            //.Include(v => v.Vehiculo) 
            .ToListAsync();
    }

    public async Task<Estado?> GetByIdAsync(int id)
    {
        return await _context.Estado.FindAsync(id);
    }

    
}
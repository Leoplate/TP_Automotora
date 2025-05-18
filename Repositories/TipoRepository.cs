using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

public class TipoRepository : ITipoRepository
{
    private readonly AppDbContext _context;

    public TipoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Tipo>> GetAllAsync()
    {
        return await _context.Tipo
            //.Include(v => v.Cliente) 
            //.Include(v => v.Vehiculo) 
            .ToListAsync();
    }

    public async Task<Tipo?> GetByIdAsync(int id)
    {
        return await _context.Tipo.FindAsync(id);
    }

    
}
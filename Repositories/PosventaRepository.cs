using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

public class PosventaRepository : IPosventaRepository
{
    private readonly AppDbContext _context;

    public PosventaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Posventa>> GetAllAsync()
    {
        return await _context.Posventas
            //.Include(v => v.Cliente) 
            //.Include(v => v.Vehiculo) 
            .ToListAsync();
    }

    public async Task<Posventa?> GetByIdAsync(int id)
    {
        return await _context.Posventas.FindAsync(id);
    }

    public async Task AddAsync(Posventa venta)
    {
        await _context.Posventas.AddAsync(venta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Posventa venta)
    {
        var existingVenta = await _context.Posventas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == venta.Id);

        if (existingVenta == null)
        {
            throw new KeyNotFoundException("El producto no existe.");
        }

        // Attach the updated entity and set its state to Modified
        _context.Attach(venta);
        _context.Entry(venta).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var venta = await _context.Posventas.FindAsync(id);
        if (venta != null)
        {
            _context.Posventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
    }


    public async Task AddAsyncRamdom(Posventa venta)
    {
        await _context.Posventas.AddAsync(venta);
        //_context.ChangeTracker.AutoDetectChangesEnabled = false;
        await _context.SaveChangesAsync();
        //_context.ChangeTracker.AutoDetectChangesEnabled = true;
    }
}
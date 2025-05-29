using technical_tests_backend_ssr.Data;
using technical_tests_backend_ssr.Models;
using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Repositories;

public class VentaRepository : IVentaRepository
{
    private readonly AppDbContext _context;

    public VentaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Venta>> GetAllAsync()
    {
        return await _context.Ventas
            .Include(v => v.Cliente) 
            .Include(v => v.Vehiculo) 
            .ToListAsync();
    }

    public async Task<Venta?> GetByIdAsync(int id)
    {

        var data = await _context.Ventas
                  .Include(v => v.Cliente)
                  .Include(v => v.Vehiculo)
                  .FirstOrDefaultAsync(v => v.Id == id);

        return data;

    } 

    public async Task AddAsync(Venta venta)
    {
        await _context.Ventas.AddAsync(venta);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Venta venta)
    {
        var existingVenta = await _context.Ventas.AsNoTracking().FirstOrDefaultAsync(p => p.Id == venta.Id);

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
        var venta = await _context.Ventas.FindAsync(id);
        if (venta != null)
        {
            _context.Ventas.Remove(venta);
            await _context.SaveChangesAsync();
        }
    }

    public async Task  AddAsyncRamdom(Venta venta)
    {
         await _context.Ventas.AddAsync(venta);
        //_context.ChangeTracker.AutoDetectChangesEnabled = false;
        await _context.SaveChangesAsync();
        //_context.ChangeTracker.AutoDetectChangesEnabled = true;
    }
    
}
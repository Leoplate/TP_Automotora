using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class VentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IClienteRepository _clienteRepository;  
    static readonly SemaphoreSlim semaforoDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPost = new SemaphoreSlim(1, 1);
    public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository, IClienteRepository clienteRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _clienteRepository = clienteRepository;
    }

    public Task<IEnumerable<Venta>> GetAllVentasAsync()
    {
        return _ventaRepository.GetAllAsync();
    }

    public Task<Venta?> GetVentaByIdAsync(int id)
    {
        return _ventaRepository.GetByIdAsync(id);
    }

    public async Task<(Venta?,string?)> AddVentaAsync(Venta venta)
    {


        var cliente = await _clienteRepository.GetByIdAsync(venta.ClienteId);
        var produ =  await _productoRepository.GetByIdAsync(venta.VehiculoId);
             

        
         if (cliente == null) return (null, "No existe cliente");

         if (produ == null) return (null, "no existe producto");

        if (venta.Total > produ.Stock) return (null, "Supera el stock del producto");

        await semaforoPost.WaitAsync();
        try
        {
            if (produ.Stock > 0)
            {
                produ.Stock = produ.Stock - venta.Total;
                await _productoRepository.UpdateAsync(produ);
                await _ventaRepository.AddAsync(venta);

                return (venta,"");
            }
        }
        finally
        {
          semaforoPost.Release();
        }

        
        

        return (null, "No hay stock del producto");
        
    }

    public async Task<Venta> UpdateClientAsync(Venta venta)
    {
        await semaforoPut.WaitAsync();
        try
        {
            await _ventaRepository.UpdateAsync(venta);
        }
        finally
        {
            semaforoPut.Release();
        }

        return venta;

    }

    public async Task<bool> DeleteVentaAsync(int id)
    {
        var existingVenta = await _ventaRepository.GetByIdAsync(id);
        
        if (existingVenta == null) return false;

          

        await semaforoDelete.WaitAsync();
        try
        {
                var produ = await _productoRepository.GetByIdAsync(existingVenta.VehiculoId);
                produ.Stock = produ.Stock + existingVenta.Total;
                await _productoRepository.UpdateAsync(produ);
                await _ventaRepository.DeleteAsync(id);
            
        }
        finally
        {
            semaforoDelete.Release();
        }
        
        return true;
    }
}

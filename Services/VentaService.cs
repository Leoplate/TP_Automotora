using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

public class VentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;

    public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
    }

    public Task<IEnumerable<Venta>> GetAllVentasAsync()
    {
        return _ventaRepository.GetAllAsync();
    }

    public Task<Venta?> GetVentaByIdAsync(int id)
    {
        return _ventaRepository.GetByIdAsync(id);
    }

    public async Task<Venta> AddVentaAsync(Venta venta)
    {
        Producto produ = await  _productoRepository.GetByIdAsync(venta.VehiculoId);

        if (produ.Stock == 0)
        {
            throw new Exception("No hay stock del producto " + produ.Nombre);
        }
        produ.Stock = produ.Stock - 1;
        await _productoRepository.UpdateAsync(produ);
        await _ventaRepository.AddAsync(venta);
        return venta;
    }

    public async Task<Venta> UpdateClientAsync(Venta venta)
    {
        await _ventaRepository.UpdateAsync(venta);
        return venta;
    }

    public async Task<bool> DeleteVentaAsync(int id)
    {
        var existingVenta = await _ventaRepository.GetByIdAsync(id);
        if (existingVenta == null) return false;

        await _ventaRepository.DeleteAsync(id);
        return true;
    }
}

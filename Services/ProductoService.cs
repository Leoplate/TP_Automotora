using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

public class ProductService
{
    private readonly IProductoRepository _productoRepository;
    static readonly SemaphoreSlim semaforoProductDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPost = new SemaphoreSlim(1, 1);
    public ProductService(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public Task<IEnumerable<Producto>> GetAllProductsAsync()
    {
        return _productoRepository.GetAllAsync();
    }

    public Task<Producto?> GetProductByIdAsync(int id)
    {
        return _productoRepository.GetByIdAsync(id);
    }

    public async Task<Producto> AddProductAsync(Producto producto)
    {
        await semaforoProductPost.WaitAsync();
        try
        {
            await _productoRepository.AddAsync(producto);
        }
        finally
        {
            semaforoProductPost.Release();
        }
        return producto;
    }

    public async Task<Producto> UpdateProductAsync(Producto producto)
    {
        await semaforoProductPut.WaitAsync();
        try
        {
            await _productoRepository.UpdateAsync(producto);
        }
        finally
        {
            semaforoProductPut.Release();
        }
        return producto;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _productoRepository.GetByIdAsync(id);
        if (existingProduct == null) return false;

        await semaforoProductDelete.WaitAsync();
        try
        {
            await _productoRepository.DeleteAsync(id);
        }
        finally
        {
            semaforoProductDelete.Release();
        }
        return true;
    }
}

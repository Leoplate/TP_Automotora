using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Threading;

public class ProductService
{
    private readonly IProductoRepository _productoRepository;
    static readonly SemaphoreSlim semaforoProductDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPost = new SemaphoreSlim(1, 1);

    //static Mutex mutexProducto = new Mutex();
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
        //mutexProducto.WaitOne();
        try
        {
            await _productoRepository.AddAsync(producto);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoProductPost.Release();
            //mutexProducto.ReleaseMutex();
        }
        return producto;
    }

    public async Task<Producto> UpdateProductAsync(Producto producto)
    {
        await semaforoProductPut.WaitAsync();
        //mutexProducto.WaitOne();
        try
        {
            await _productoRepository.UpdateAsync(producto);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoProductPut.Release();
            //mutexProducto.ReleaseMutex();
        }
        return producto;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var existingProduct = await _productoRepository.GetByIdAsync(id);
        if (existingProduct == null) return false;

        await semaforoProductDelete.WaitAsync();
        //mutexProducto.WaitOne();
        try
        {
            await _productoRepository.DeleteAsync(id);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoProductDelete.Release();
            //mutexProducto.ReleaseMutex();
        }
        return true;
    }
}

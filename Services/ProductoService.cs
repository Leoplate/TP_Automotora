using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ProductService
{
    private readonly IProductoRepository _productoRepository;
    static readonly SemaphoreSlim semaforoProductDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductPost = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoProductTest = new SemaphoreSlim(1, 1);

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


    public async Task TestProductos(IEnumerable<Producto> productos)
    {
        var test = productos
            .AsParallel()
            .GroupBy(v => v.Nombre)
            .SelectMany(g => Enumerable.Range(1, g.First().Stock) 
                .Select(i => new { Modelo = g.Key, Instancia = i }))
            .ToList();

        var tasks = test.Select(async d =>
        {
            await semaforoProductTest.WaitAsync(); 
            try
            {
                await Testear(d.Modelo, d.Instancia);
            }
            finally
            {
                semaforoProductTest.Release();
            }
        }).ToArray();

        await Task.WhenAll(tasks);
    }

    static async Task Testear(string modelo, int instancia)
    {
        Console.WriteLine($"Esperando ({instancia}): {modelo}");
        await Task.Delay(1000); 
        Console.WriteLine($"Testeando ({instancia}): {modelo}");
        await Task.Delay(3000); 
    }




}

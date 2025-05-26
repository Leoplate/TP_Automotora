using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;

public class PosventaService
{
    private readonly IPosventaRepository _posventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IEstadoRepository _estadoRepository;
    private readonly ITipoRepository _tipoRepository;

    static readonly SemaphoreSlim semaforoDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPost = new SemaphoreSlim(1, 1);
    //static Mutex ventaMutex = new Mutex(); 
    public PosventaService(IPosventaRepository posventaRepository, IProductoRepository productoRepository, IClienteRepository clienteRepository, IEstadoRepository estadoRepository, ITipoRepository tipoRepository)
    {
        _posventaRepository = posventaRepository;
        _productoRepository = productoRepository;
        _clienteRepository = clienteRepository;
        _estadoRepository = estadoRepository;
        _tipoRepository = tipoRepository;
    }

    public Task<IEnumerable<Posventa>> GetAllPostventasAsync()
    {
        return _posventaRepository.GetAllAsync();
    }

    public Task<Posventa?> GetPosventaByIdAsync(int id)
    {
        return _posventaRepository.GetByIdAsync(id);
    }

    public async Task<Posventa> AddPosventaAsync(Posventa posventa)
    {


            

        //var cliente = await _clienteRepository.GetByIdAsync(posventa.ClienteId);
        


        
        

        //if (cliente == null) return (null, "No existe cliente");

        //var estado = await _estadoRepository.GetByIdAsync(posventa.Estadoid);
        //if (estado == null) return (null, "no existe estado Posventa n° "+posventa.Estadoid);

        //var tipo = await _tipoRepository.GetByIdAsync(posventa.Tipoid);
        //if (tipo == null) return (null, "no existe tipo Posventa n° "+posventa.Tipoid);
        //if (venta.Total > produ.Stock) return (null, "Supera el stock del producto");

        await semaforoPost.WaitAsync();
        //ventaMutex.WaitOne();
        try
        {
            //if (produ.Stock > 0)
            //{
                //produ.Stock = produ.Stock - venta.Total;
                //await _productoRepository.UpdateAsync(produ);

                await _posventaRepository.AddAsync(posventa);

                return (posventa);
            //}
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoPost.Release();
            //ventaMutex.ReleaseMutex();
        }

        
        

        
        
    }

    public async Task<Posventa> UpdatePosventaAsync(Posventa posventa)
    {
        await semaforoPut.WaitAsync();
        //ventaMutex.WaitOne();
        
        try
        {
            await _posventaRepository.UpdateAsync(posventa);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoPut.Release();
            //ventaMutex.ReleaseMutex();
        }

        return posventa;

    }

    public async Task<bool> DeletePosventaAsync(int id)
    {
        var existingPosventa = await _posventaRepository.GetByIdAsync(id);
        
        if (existingPosventa == null) return false;



        await semaforoDelete.WaitAsync();
        //ventaMutex.WaitOne();
        try
        {
                //var produ = await _productoRepository.GetByIdAsync(existingVenta.VehiculoId);
                //produ.Stock = produ.Stock + existingVenta.Total;
                //await _productoRepository.UpdateAsync(produ);
                await _posventaRepository.DeleteAsync(id);
            
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        finally
        {
            semaforoDelete.Release();
            //ventaMutex.ReleaseMutex();
        }
        
        return true;
    }



    public async Task AddPosventaAsyncRandom()
    {
        Random random = new Random();

        for (int i = 0; i < 5000; i++)
        {
            //Parallel.For(0, 1000, i =>
            //{
            Posventa posRandom = new Posventa
            {
                ClienteId = random.Next(1, 4), 
                Tipoid = random.Next(1, 4),
                Fecha = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(random.Next(-365, 0))), // Fecha aleatoria en un rango de un año
                Estadoid = random.Next(1, 3)
            };

            await _posventaRepository.AddAsyncRamdom(posRandom);
        }
        //}
        //await Task.WhenAll(tarea);
        //});
        //var venta = _mapper.Map<Venta>(posRandom);
        //Task.Run(() => _ventaRepository.AddAsyncRamdom(posRandom));
        //await _ventaRepository.AddAsyncRamdom(posRandom);
        /// }
    }








}



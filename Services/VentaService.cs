using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using AutoMapper;
using System.Formats.Tar;


public class VentaService
{
    private readonly IVentaRepository _ventaRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly IMapper _mapper;
    static readonly SemaphoreSlim semaforoDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoPost = new SemaphoreSlim(1, 1);
    //static Mutex ventaMutex = new Mutex(); 

    

    public VentaService(IVentaRepository ventaRepository, IProductoRepository productoRepository, IClienteRepository clienteRepository, IMapper mapper)
    {
        _ventaRepository = ventaRepository;
        _productoRepository = productoRepository;
        _clienteRepository = clienteRepository;
        _mapper = mapper;
    }   

    public Task<IEnumerable<Venta>> GetAllVentasAsync()
    {
        return _ventaRepository.GetAllAsync();
    }

    public Task<Venta?> GetVentaByIdAsync(int id)
    {
        return _ventaRepository.GetByIdAsync(id);
    }

    //public async Task<(Venta?,string?)> AddVentaAsync(Venta venta)
    public async Task<Venta?> AddVentaAsync(Venta venta)
    {


        var cliente = await _clienteRepository.GetByIdAsync(venta.ClienteId);
        var produ =  await _productoRepository.GetByIdAsync(venta.VehiculoId);





        //if (cliente == null) return (null, "No existe cliente");

        //if (produ == null) return (null, "no existe producto");

        //if (venta.Total > produ.Stock) return (null, "Supera el stock del producto");
        if (venta.Total <= produ.Stock)
        {
            await semaforoPost.WaitAsync();
            //ventaMutex.WaitOne();
            try
            {
                //if (produ.Stock > 0)
                if (venta.Total > 0)
                {
                    produ.Stock = produ.Stock - venta.Total;
                    await _productoRepository.UpdateAsync(produ);
                    await _ventaRepository.AddAsync(venta);

                    //return (venta,"Venta Realizada");
                    return (venta);
                }
            }
            finally
            {
                semaforoPost.Release();
                //ventaMutex.ReleaseMutex();
            }

        }

        return (null);
        //return (null, "No hay stock del producto");
        
    }

    public async Task<Venta> UpdateVentaAsync(Venta venta)
    {
        await semaforoPut.WaitAsync();
        //ventaMutex.WaitOne();
        try
        {
            await _ventaRepository.UpdateAsync(venta);
        }
        finally
        {
            semaforoPut.Release();
            //ventaMutex.ReleaseMutex();
        }

        return venta;

    }

    public async Task<bool> DeleteVentaAsync(int id)
    {
        var existingVenta = await _ventaRepository.GetByIdAsync(id);
        
        if (existingVenta == null) return false;



        await semaforoDelete.WaitAsync();
        //ventaMutex.WaitOne();
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
            Venta posRandom = new Venta
            {
                ClienteId = random.Next(1, 4), // Genera un número aleatorio entre 1 y 3
                VehiculoId = random.Next(1, 4),
                Fecha = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(random.Next(-365, 0))), // Fecha aleatoria en un rango de un año
                Total = random.Next(1, 3)
            };

            await _ventaRepository.AddAsyncRamdom(posRandom);
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

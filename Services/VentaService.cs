using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using AutoMapper;
using System.Formats.Tar;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Data;
using Microsoft.Extensions.Options;



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



    public VentaService(IVentaRepository ventaRepository,  IProductoRepository productoRepository,  IClienteRepository clienteRepository, IMapper mapper)
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


        //var cliente = await _clienteRepository.GetByIdAsync(venta.ClienteId);
         var produ = await _productoRepository.GetByIdAsync(venta.VehiculoId);

        



        //if (cliente == null) return (null, "No existe cliente");

        //if (produ == null) return (null, "no existe producto");

        //if (venta.Total > produ.Stock) return (null, "Supera el stock del producto");
       
        

        if (venta.Total <= produ.Stock)
        {
            await semaforoPost.WaitAsync();
            //await semaforoPost.WaitAsync();
            //ventaMutex.WaitOne();
            try
            {
                //if (produ.Stock > 0)
                //if (venta.Total > 0)
                //{
                    produ.Stock = produ.Stock - venta.Total;
                    await _productoRepository.UpdateAsync(produ);
                    await _ventaRepository.AddAsync(venta);
                    //var tarea1 = _productoRepository.UpdateAsync(produ);
                    //var tarea2 = _ventaRepository.AddAsync(venta);
                    //return (venta,"Venta Realizada");
                    //await Task.WhenAll(tarea1, tarea2);

                    return (venta);
                //}
                //else
                //{
                //    throw new InvalidOperationException("La cantidad de venta debe ser mayor a 0.");
                //}
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                semaforoPost.Release();
                //semaforoPost.Release();

                //ventaMutex.ReleaseMutex();
            }


        }
        else
        {
           return null;
        }

        
        
    }

    public async Task<Venta?> UpdateVentaAsync(Venta venta, int anterior)
    {

        var final = 0;
        var produ = await _productoRepository.GetByIdAsync(venta.VehiculoId);

        
        //var actual =  await _ventaRepository.GetByIdAsync(venta.Id);
        //if(actual == null) { return null; }

        if (venta.Total <= anterior)
        {
            final = venta.Total - anterior;
        }
        else
        {
            final = venta.Total - anterior;
        }



        if (final <= produ.Stock)
        {





            await semaforoPut.WaitAsync();
            
            try
            {

                produ.Stock = produ.Stock - final;
                await _productoRepository.UpdateAsync(produ);
                await _ventaRepository.UpdateAsync(venta);

                return venta;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                semaforoPut.Release();
                

            }
        }
        else
        {
            return null;

        }


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
            
            //var tarea1 = Task.Run(() => _productoRepository.UpdateAsync(produ));
            //var tarea2 = Task.Run(() => _ventaRepository.DeleteAsync(id));
            await _productoRepository.UpdateAsync(produ);
            await _ventaRepository.DeleteAsync(id);
            //await Task.WhenAll(tarea1, tarea2);
            return true;
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
        
        
    }


    public async Task <List<TopVehiculoDTO>> GenerarListadoVehiculos(IEnumerable<VentaCompletaDTO> ventas)
    {
       var vehiculos = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas)
        .AsParallel()
       .GroupBy(v => v.ProductoNombre)
       .Select(g => new
       {
           VehiculoId = g.Key,
           TotalVentas = g.Count()
       })
       .OrderByDescending(e => e.TotalVentas)
       .Take(5)
       .ToList();


        List<TopVehiculoDTO> top = new List<TopVehiculoDTO>();

        foreach (var vehiculo in vehiculos)
        {
            var topVehiculo = new TopVehiculoDTO
            {
                Vehiculo = vehiculo.VehiculoId,
                Total = vehiculo.TotalVentas,
            };
            top.Add(topVehiculo);
        }

        return top;


    }


    public async Task<List<TopClienteDTO>> GenerarListadoClientes(IEnumerable<VentaCompletaDTO> ventas)
    {
        var clientes = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas)
       .AsParallel()
       .GroupBy(v => new { v.ClienteId, v.ClienteNombre, v.ClienteApellido })
       .Select(g => new
       {
           ClienteId = g.Key,
           Nombre = g.Key.ClienteNombre + " " + g.Key.ClienteApellido,
           TotalVentas = g.Count()
       })
       .OrderByDescending(e => e.TotalVentas)
       .Take(5)
       .ToList();






        List<TopClienteDTO> top = new List<TopClienteDTO>();

        foreach (var cliente in clientes)
        {
            var topcliente = new TopClienteDTO
            {
                Nombre = cliente.Nombre,
                Total = cliente.TotalVentas,
            };
            top.Add(topcliente);
        }

        //return Ok(top);
        return top;
    }



    public async Task<List<TopAnioDTO>> GenerarListadoVentasAnuales(IEnumerable<VentaCompletaDTO> ventas)
    {



        var anios = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas)
        .AsParallel()
        .GroupBy(v => v.Fecha.Year)
        .Select(g => new
        {
            Year = g.Key,
            
        })
        .Take(5)
        .ToList();


        var vehiculos = _mapper.Map<IEnumerable<VentaCompletaDTO>>(ventas);

        var tasks = anios.Select(d => Task.Run(() =>  Calculo(d.Year,vehiculos)));

        
        

        

        

        

        var resultados = await Task.WhenAll(tasks);

        // Retornar los resultados combinados en una lista plana
        return resultados.SelectMany(r => r).Cast<TopAnioDTO>().ToList();




        static async Task<List<TopAnioDTO>>  Calculo(int anio, IEnumerable<VentaCompletaDTO> listado)
        {
         
            
         var datos = listado
         .AsParallel()
         .Where(v => v.Fecha.Year == anio)
        .GroupBy(v => v.Fecha.Year )
        .Select(g => new
        {
            Year = g.Key,
            TotalVentas = g.Count()
        })
        .OrderByDescending(e => e.TotalVentas)
        //.Take(5)
        .ToList();




            List<TopAnioDTO> top = new List<TopAnioDTO>();

            foreach (var dato in datos)
            {
                var topAnio = new TopAnioDTO
                {
                    Anio = dato.Year,
                    Total = dato.TotalVentas,
                };
                top.Add(topAnio);
            }


            return await Task.FromResult(top);
        }


    }



    //Generación de ventas aleatorias

    public async Task AddPosventaAsyncRandom()
    {
        Random random = new Random();


        for (int i = 0; i < 1000; i++)
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

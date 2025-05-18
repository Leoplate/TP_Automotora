using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using System.Threading;


public class ClientService
{
    private readonly IClienteRepository _clienteRepository;
    static readonly SemaphoreSlim semaforoClientDelete = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoClientPut = new SemaphoreSlim(1, 1);
    static readonly SemaphoreSlim semaforoClientPost = new SemaphoreSlim(1, 1);
    //static Mutex mutexCliente = new Mutex();


    public ClientService(IClienteRepository clienteRepository)
    {
        _clienteRepository = clienteRepository;
    }

    public Task<IEnumerable<Cliente>> GetAllClientsAsync()
    {
        return _clienteRepository.GetAllAsync();
    }

    public Task<Cliente?> GetClientByIdAsync(int id)
    {
        return _clienteRepository.GetByIdAsync(id);
    }

    public async Task<Cliente> AddClientAsync(Cliente cliente)
    {


        await semaforoClientPost.WaitAsync();
        //mutexCliente.WaitOne();
        try
        {
           await _clienteRepository.AddAsync(cliente);
        }
        finally
        {
          semaforoClientPost.Release();
          //mutexCliente.ReleaseMutex();
        }
       return cliente;
    }

    public async Task<Cliente> UpdateClientAsync(Cliente cliente)
    {
        await semaforoClientPut.WaitAsync();
        //mutexCliente.WaitOne(); 
        try
        {
            await _clienteRepository.UpdateAsync(cliente);
        }
        finally
        {
            semaforoClientPut.Release();
            //mutexCliente.ReleaseMutex();
        }
        return cliente;
    }

    public async Task<bool> DeleteClientAsync(int id)
    {
        var existingClient = await _clienteRepository.GetByIdAsync(id);
        if (existingClient == null) return false;


        await semaforoClientDelete.WaitAsync();
        //mutexCliente.WaitOne();
        try
        {
            await _clienteRepository.DeleteAsync(id);
        }
        finally
        {
            semaforoClientDelete.Release();
            //mutexCliente.ReleaseMutex();
        }
            return true;
    }
}

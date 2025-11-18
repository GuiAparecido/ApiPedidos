using System.Collections.Generic;
using System.Threading.Tasks;
using OrderSystemApi.Models;

namespace OrderSystemApi.Services.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(string id);
        Task<Cliente> CreateAsync(Cliente cliente);
        Task<bool> UpdateAsync(string id, Cliente cliente);
        Task<bool> DeleteAsync(string id);
    }
}

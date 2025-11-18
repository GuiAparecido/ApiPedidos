using System.Collections.Generic;
using System.Threading.Tasks;
using OrderSystemApi.Models;

namespace OrderSystemApi.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<Usuario>> GetAllAsync();
        Task<Usuario?> GetByIdAsync(string id);
        Task<Usuario> CreateAsync(Usuario usuario);
        Task<bool> UpdateAsync(string id, Usuario usuario);
        Task<bool> DeleteAsync(string id);
    }
}

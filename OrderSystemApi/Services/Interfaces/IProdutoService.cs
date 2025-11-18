using System.Collections.Generic;
using System.Threading.Tasks;
using OrderSystemApi.Models;

namespace OrderSystemApi.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<IEnumerable<Produto>> GetAllAsync();
        Task<Produto?> GetByIdAsync(string id);
        Task<Produto> CreateAsync(Produto produto);
        Task<bool> UpdateAsync(string id, Produto produto);
        Task<bool> DeleteAsync(string id);
    }
}

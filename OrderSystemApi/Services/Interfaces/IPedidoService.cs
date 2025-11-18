using OrderSystemApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderSystemApi.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<IEnumerable<Pedido>> GetAllAsync();
        Task<Pedido?> GetByIdAsync(string id);
        Task<IEnumerable<Pedido>> GetByStatusAsync(StatusPedido status);
        Task<Pedido> CreateAsync(PedidoCreateDto dto);
        Task<bool> MarkAsPaidAsync(string id);
        Task<bool> CancelAsync(string id);
        Task<decimal> GetTotalAsync(string id);

    }
}

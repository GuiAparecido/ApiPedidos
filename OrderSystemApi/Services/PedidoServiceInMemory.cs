using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;

namespace OrderSystemApi.Services
{
    public class PedidoServiceInMemory : IPedidoService
    {
        private static readonly List<Pedido> _pedidos = new();
        private static readonly object _lock = new();

        private readonly IClienteService _clienteService;
        private readonly IProdutoService _produtoService;

        public PedidoServiceInMemory(IClienteService clienteService, IProdutoService produtoService)
        {
            _clienteService = clienteService;
            _produtoService = produtoService;
        }

        public Task<IEnumerable<Pedido>> GetAllAsync()
        {
            IEnumerable<Pedido> copy;
            lock (_lock) { copy = _pedidos.ToList(); }
            return Task.FromResult(copy);
        }

        public Task<Pedido?> GetByIdAsync(string id)
        {
            lock (_lock)
            {
                return Task.FromResult(_pedidos.FirstOrDefault(p => p.Id == id));
            }
        }

        public Task<IEnumerable<Pedido>> GetByStatusAsync(StatusPedido status)
        {
            IEnumerable<Pedido> result;
            lock (_lock)
            {
                result = _pedidos.Where(p => p.Status == status).ToList();
            }
            return Task.FromResult(result);
        }

        public async Task<Pedido> CreateAsync(PedidoCreateDto dto)
        {
            if (dto == null)
                throw new InvalidOperationException("DTO inválido.");
            if (string.IsNullOrEmpty(dto.ClienteId))
                throw new InvalidOperationException("ClienteId é obrigatório.");
            if (dto.Produtos == null || dto.Produtos.Count == 0)
                throw new InvalidOperationException("Pedido precisa ter ao menos 1 produto.");

            // valida entradas individuais
            if (dto.Produtos.Any(p => p.Quantidade < 1))
                throw new InvalidOperationException("Cada produto precisa ter quantidade >= 1.");

            // validar cliente
            var cliente = await _clienteService.GetByIdAsync(dto.ClienteId);
            if (cliente == null) throw new InvalidOperationException("Cliente não encontrado.");

            var itens = new List<ProdutoItem>();
            var faltando = new List<string>();

            // agrupar por id e somar quantidades
            var agrupado = dto.Produtos
                .GroupBy(p => p.Id)
                .Select(g => new { Id = g.Key, Quantidade = g.Sum(x => x.Quantidade) })
                .ToList();

            foreach (var entrada in agrupado)
            {
                var produto = await _produtoService.GetByIdAsync(entrada.Id);
                if (produto == null) { faltando.Add(entrada.Id); continue; }

                if (entrada.Quantidade <= 0)
                    throw new InvalidOperationException($"Quantidade inválida para o produto {entrada.Id}. Deve ser >= 1.");

                itens.Add(new ProdutoItem
                {
                    Id = produto.Id,
                    Nome = produto.Nome,
                    Preco = produto.Preco,
                    Quantidade = entrada.Quantidade
                });
            }

            if (faltando.Any())
                throw new InvalidOperationException($"Produtos não encontrados: {string.Join(", ", faltando)}");

            var pedido = new Pedido
            {
                Id = Guid.NewGuid().ToString(),
                ClienteId = dto.ClienteId,
                ClienteNome = cliente.Nome,      // <-- aqui
                Produtos = itens,
                Data = DateTime.UtcNow,
                Status = StatusPedido.Criado,
                Historico = new List<string>
    {
        $"{DateTime.UtcNow:O} - Pedido criado"
    }
            };

            lock (_lock)
            {
                _pedidos.Add(pedido);
            }

            return pedido;
        }

        public Task<bool> MarkAsPaidAsync(string id)
        {
            lock (_lock)
            {
                var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
                if (pedido == null) return Task.FromResult(false);
                if (pedido.Status == StatusPedido.Pago) return Task.FromResult(false);

                pedido.Status = StatusPedido.Pago;
                pedido.Historico.Add($"{DateTime.UtcNow:O} - Pedido pago");

                return Task.FromResult(true);
            }
        }

        public Task<bool> CancelAsync(string id)
        {
            lock (_lock)
            {
                var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
                if (pedido == null) return Task.FromResult(false);
                if (pedido.Status == StatusPedido.Pago)
                    return Task.FromResult(false);

                pedido.Status = StatusPedido.Cancelado;
                pedido.Historico.Add($"{DateTime.UtcNow:O} - Pedido cancelado");

                return Task.FromResult(true);
            }
        }

        public Task<decimal> GetTotalAsync(string id)
        {
            lock (_lock)
            {
                var pedido = _pedidos.FirstOrDefault(p => p.Id == id);
                if (pedido == null)
                    throw new InvalidOperationException("Pedido não encontrado.");

                return Task.FromResult(pedido.Total);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;

namespace OrderSystemApi.Services
{
    public class ProdutoServiceInMemory : IProdutoService
    {
        private static readonly List<Produto> _produtos = new();
        private static readonly object _lock = new();

        public Task<IEnumerable<Produto>> GetAllAsync()
        {
            IEnumerable<Produto> copy;
            lock (_lock) { copy = _produtos.ToList(); }
            return Task.FromResult(copy);
        }

        public Task<Produto?> GetByIdAsync(string id)
        {
            lock (_lock)
            {
                return Task.FromResult(_produtos.FirstOrDefault(p => p.Id == id));
            }
        }

        public Task<Produto> CreateAsync(Produto produto)
        {
            produto.Id = Guid.NewGuid().ToString();
            produto.Historico.Add($"{DateTime.UtcNow:O} - Produto criado");
            lock (_lock)
            {
                _produtos.Add(produto);
            }
            return Task.FromResult(produto);
        }

        public Task<bool> UpdateAsync(string id, Produto produto)
        {
            lock (_lock)
            {
                var idx = _produtos.FindIndex(p => p.Id == id);
                if (idx == -1) return Task.FromResult(false);

                var existing = _produtos[idx];
                existing.Nome = produto.Nome;
                existing.Preco = produto.Preco;
                existing.Historico.Add($"{DateTime.UtcNow:O} - Produto atualizado");
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteAsync(string id)
        {
            lock (_lock)
            {
                var produto = _produtos.FirstOrDefault(p => p.Id == id);
                if (produto == null) return Task.FromResult(false);
                produto.Historico.Add($"{DateTime.UtcNow:O} - Produto removido");
                var removed = _produtos.RemoveAll(p => p.Id == id) > 0;
                return Task.FromResult(removed);
            }
        }
    }
}


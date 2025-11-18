using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;

namespace OrderSystemApi.Services
{
    public class ClienteServiceInMemory : IClienteService
    {
        private static readonly List<Cliente> _clientes = new();
        private static readonly object _lock = new();

        public Task<IEnumerable<Cliente>> GetAllAsync()
        {
            IEnumerable<Cliente> copy;
            lock (_lock) { copy = _clientes.Select(c => c).ToList(); }
            return Task.FromResult(copy);
        }

        public Task<Cliente?> GetByIdAsync(string id)
        {
            lock (_lock)
            {
                return Task.FromResult(_clientes.FirstOrDefault(c => c.Id == id));
            }
        }

        public Task<Cliente> CreateAsync(Cliente cliente)
        {
            cliente.Id = Guid.NewGuid().ToString();
            cliente.Historico.Add($"{DateTime.UtcNow:O} - Cliente criado");
            lock (_lock)
            {
                _clientes.Add(cliente);
            }
            return Task.FromResult(cliente);
        }

        public Task<bool> UpdateAsync(string id, Cliente cliente)
        {
            lock (_lock)
            {
                var idx = _clientes.FindIndex(c => c.Id == id);
                if (idx == -1) return Task.FromResult(false);

                // preserve existing history and append a new entry
                var existing = _clientes[idx];
                existing.Nome = cliente.Nome;
                existing.CPF = cliente.CPF;
                existing.Historico.Add($"{DateTime.UtcNow:O} - Cliente atualizado");
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteAsync(string id)
        {
            lock (_lock)
            {
                var cliente = _clientes.FirstOrDefault(c => c.Id == id);
                if (cliente == null) return Task.FromResult(false);
                // record deletion time in history (note: after this the entity is removed)
                cliente.Historico.Add($"{DateTime.UtcNow:O} - Cliente removido");
                var removed = _clientes.RemoveAll(c => c.Id == id) > 0;
                return Task.FromResult(removed);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;

namespace OrderSystemApi.Services
{
    public class UsuarioServiceInMemory : IUsuarioService
    {
        private static readonly List<Usuario> _usuarios = new();
        private static readonly object _lock = new();

        public Task<IEnumerable<Usuario>> GetAllAsync()
        {
            IEnumerable<Usuario> copy;
            lock (_lock) { copy = _usuarios.ToList(); }
            return Task.FromResult(copy);
        }

        public Task<Usuario?> GetByIdAsync(string id)
        {
            lock (_lock)
            {
                return Task.FromResult(_usuarios.FirstOrDefault(u => u.Id == id));
            }
        }

        public Task<Usuario> CreateAsync(Usuario usuario)
        {
            usuario.Id = Guid.NewGuid().ToString();
            usuario.Historico.Add($"{DateTime.UtcNow:O} - Usuário criado");
            lock (_lock)
            {
                _usuarios.Add(usuario);
            }
            return Task.FromResult(usuario);
        }

        public Task<bool> UpdateAsync(string id, Usuario usuario)
        {
            lock (_lock)
            {
                var idx = _usuarios.FindIndex(u => u.Id == id);
                if (idx == -1) return Task.FromResult(false);

                var existing = _usuarios[idx];
                existing.Nome = usuario.Nome;
                existing.Historico.Add($"{DateTime.UtcNow:O} - Usuário atualizado");
                return Task.FromResult(true);
            }
        }

        public Task<bool> DeleteAsync(string id)
        {
            lock (_lock)
            {
                var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null) return Task.FromResult(false);
                usuario.Historico.Add($"{DateTime.UtcNow:O} - Usuário removido");
                var removed = _usuarios.RemoveAll(u => u.Id == id) > 0;
                return Task.FromResult(removed);
            }
        }
    }
}

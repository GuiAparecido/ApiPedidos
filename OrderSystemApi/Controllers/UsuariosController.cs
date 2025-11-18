using Microsoft.AspNetCore.Mvc;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;
using System.Threading.Tasks;

namespace OrderSystemApi.Controllers
{
    [ApiController]
    [Route("usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _service;

        public UsuariosController(IUsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _service.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var u = await _service.GetByIdAsync(id);
            if (u == null) return NotFound();
            return Ok(u);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Usuario usuario)
        {
            var created = await _service.CreateAsync(usuario);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Usuario updated)
        {
            var ok = await _service.UpdateAsync(id, updated);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}

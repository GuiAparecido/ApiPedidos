using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderSystemApi.Models;
using OrderSystemApi.Services.Interfaces;
using System.Linq;

namespace OrderSystemApi.Controllers
{
    [ApiController]
    [Route("pedidos")]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _service;

        public PedidosController(IPedidoService service)
        {
            _service = service;
        }

        // GET /pedidos
        [HttpGet]
        public async Task<IActionResult> Get() =>
            Ok(await _service.GetAllAsync());

        // GET /pedidos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var p = await _service.GetByIdAsync(id);
            if (p == null) return NotFound(new { erro = "Pedido não encontrado." });
            return Ok(p);
        }

        // GET /pedidos/{id}/status
        // Retorna um JSON com { id, status }
        [HttpGet("{id}/status")]
        public async Task<IActionResult> GetStatusById(string id)
        {
            var pedido = await _service.GetByIdAsync(id);
            if (pedido == null) return NotFound(new { erro = "Pedido não encontrado." });

            return Ok(new { id = pedido.Id, status = pedido.Status.ToString() });
        }

        // GET /pedidos/status/{status}
        // Lista todos os pedidos que possuem o status informado (Criado, Pago, Cancelado)
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetByStatus(string status)
        {
            if (!Enum.TryParse(typeof(StatusPedido), status, true, out var parsed))
                return BadRequest(new { erro = "Status inválido. Use: Criado, Pago, Cancelado." });

            var s = (StatusPedido)parsed;
            var pedidos = await _service.GetByStatusAsync(s);
            return Ok(pedidos);
        }

        // POST /pedidos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PedidoCreateDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ClienteId) || dto.Produtos == null || dto.Produtos.Count == 0)
                return BadRequest(new { erro = "Payload inválido: precisa de clienteId e lista de produtos." });

            try
            {
                // delega a criação ao service (service deve mapear quantidade e calcular total)
                var pedido = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = pedido.Id }, pedido);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { erro = ex.Message });
            }
            catch (Exception ex)
            {
                // log se tiver logger (não expor detalhes sensíveis)
                return StatusCode(500, new { erro = "Erro interno ao criar pedido." });
            }
        }

        // POST /pedidos/{id}/pagar
        [HttpPost("{id}/pagar")]
        public async Task<IActionResult> Pagar(string id)
        {
            var ok = await _service.MarkAsPaidAsync(id);
            if (!ok) return NotFound(new { erro = "Pedido não encontrado ou já está pago." });
            return NoContent();
        }

        // POST /pedidos/{id}/cancelar
        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> Cancelar(string id)
        {
            var ok = await _service.CancelAsync(id);
            if (!ok) return BadRequest(new { erro = "Não foi possível cancelar. Pode já estar pago ou não existir." });
            return NoContent();
        }

        // GET /pedidos/{id}/total
        [HttpGet("{id}/total")]
        public async Task<IActionResult> Total(string id)
        {
            try
            {
                var total = await _service.GetTotalAsync(id);
                return Ok(total);
            }
            catch (InvalidOperationException)
            {
                return NotFound(new { erro = "Pedido não encontrado." });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using OrderSystemApi.Models;
using OrderSystemApi.Services;
using OrderSystemApi.Services.Interfaces;
using Xunit;

namespace OrderSystemApi.Tests
{
    public class PedidoServiceTests
    {
        private readonly IClienteService _clienteService = new ClienteServiceInMemory();
        private readonly IProdutoService _produtoService = new ProdutoServiceInMemory();
        private readonly IPedidoService _pedidoService;

        public PedidoServiceTests()
        {
            _pedidoService = new PedidoServiceInMemory(_clienteService, _produtoService);
        }

        [Fact]
        public async Task TotalDoPedido_DeveRetornarSomaDosProdutos()
        {
            var cliente = await _clienteService.CreateAsync(new Cliente { Nome = "ClienteTotal", CPF = "000" });
            var p1 = await _produtoService.CreateAsync(new Produto { Nome = "P1", Preco = 10m });
            var p2 = await _produtoService.CreateAsync(new Produto { Nome = "P2", Preco = 15.5m });

            var dto = new PedidoCreateDto
            {
                ClienteId = cliente.Id,
                Produtos = new List<ProdutoItem>
                {
                    new ProdutoItem { Id = p1.Id, Nome = p1.Nome, Preco = p1.Preco },
                    new ProdutoItem { Id = p2.Id, Nome = p2.Nome, Preco = p2.Preco }
                }
            };

            var pedido = await _pedidoService.CreateAsync(dto);
            var total = await _pedidoService.GetTotalAsync(pedido.Id);

            total.Should().Be(25.5m);
        }

        [Fact]
        public async Task CriarPedido_ComClienteInexistente_DeveLancarInvalidOperationException()
        {
            var dto = new PedidoCreateDto
            {
                ClienteId = "id-inexistente",
                Produtos = new List<ProdutoItem>
                {
                    new ProdutoItem { Id = "qualquer", Nome = "X", Preco = 1m }
                }
            };

            Func<Task> act = async () => await _pedidoService.CreateAsync(dto);
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task CriarPedido_SemProdutos_DeveLancarInvalidOperationException()
        {
            var cliente = await _clienteService.CreateAsync(new Cliente { Nome = "SemProdutos", CPF = "111" });

            var dto = new PedidoCreateDto
            {
                ClienteId = cliente.Id,
                Produtos = new List<ProdutoItem>() // vazio
            };

            Func<Task> act = async () => await _pedidoService.CreateAsync(dto);
            await act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}

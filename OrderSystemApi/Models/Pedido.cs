namespace OrderSystemApi.Models
{
    public class Pedido
    {
        public string Id { get; set; } = "";
        public string ClienteId { get; set; } = "";

        // <-- nova propriedade
        public string ClienteNome { get; set; } = "";

        public List<ProdutoItem> Produtos { get; set; } = new List<ProdutoItem>();
        public DateTime Data { get; set; }
        public StatusPedido Status { get; set; } = StatusPedido.Criado;
        public List<string> Historico { get; set; } = new List<string>();

        public decimal Total => Produtos?.Sum(p => p.Preco * (p.Quantidade > 0 ? p.Quantidade : 1)) ?? 0m;
    }
}

namespace OrderSystemApi.Models
{
    public class Produto
    {
        public string Id { get; set; } = "";
        public string Nome { get; set; } = "";
        public decimal Preco { get; set; }
        public List<string> Historico { get; set; } = new List<string>();
    }
}


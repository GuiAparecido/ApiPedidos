namespace OrderSystemApi.Models
{
    public class Cliente
    {
        public string Id { get; set; } = "";
        public string Nome { get; set; } = "";
        public string CPF { get; set; } = "";
        public List<string> Historico { get; set; } = new List<string>();
    }
}

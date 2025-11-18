namespace OrderSystemApi.Models
{
    public class Usuario
    {
        public string Id { get; set; } = "";
        public string Nome { get; set; } = "";
        public List<string> Historico { get; set; } = new List<string>();
    }
}

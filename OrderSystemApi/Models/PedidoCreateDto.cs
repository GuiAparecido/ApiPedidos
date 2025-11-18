public class ProdutoPedidoDto
{
    public string Id { get; set; } = "";
    public int Quantidade { get; set; } = 1;
}

public class PedidoCreateDto
{
    public string ClienteId { get; set; } = "";
    public List<ProdutoPedidoDto> Produtos { get; set; } = new();
}

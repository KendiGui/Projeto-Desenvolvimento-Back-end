namespace Domain.Contracts.Requests
{
    public record ItemPedidoRequest
    {
        public long ProdutoId { get; init; }
        public int Quantidade { get; init; }
    }
}

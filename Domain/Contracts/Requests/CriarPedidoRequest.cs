namespace Domain.Contracts.Requests
{
    public record CriarPedidoRequest
    {
        public long UnidadeId { get; init; }
        public string CanalPedido { get; init; }
        public string FormaPagamento { get; init; } = "MOCK";
        public IEnumerable<ItemPedidoRequest> Itens { get; init; } = [];
    }
}

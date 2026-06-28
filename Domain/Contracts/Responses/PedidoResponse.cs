namespace Domain.Contracts.Responses
{
    public record PedidoResponse
    {
        public long PedidoId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string CanalPedido { get; init; } = string.Empty;
        public long UnidadeId { get; init; }
        public long ClienteId { get; init; }
        public decimal Total { get; init; }
        public DateTime CreatedAt { get; init; }
        public PagamentoResponse? Pagamento { get; init; }
        public IEnumerable<ItemPedidoResponse> Itens { get; init; } = [];
    }
}

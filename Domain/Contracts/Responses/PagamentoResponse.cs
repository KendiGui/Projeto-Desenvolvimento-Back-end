namespace Domain.Contracts.Responses
{
    public record PagamentoResponse
    {
        public long Id { get; init; }
        public long PedidoId { get; init; }
        public string Status { get; init; } = string.Empty;
        public string FormaPagamento { get; init; } = string.Empty;
        public string TransactionId { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }
}

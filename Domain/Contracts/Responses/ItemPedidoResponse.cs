namespace Domain.Contracts.Responses
{
    public record ItemPedidoResponse
    {
        public long ProdutoId { get; init; }
        public string Nome { get; init; } = string.Empty;
        public int Quantidade { get; init; }
        public decimal PrecoUnitario { get; init; }
        public decimal Subtotal { get; init; }
    }
}

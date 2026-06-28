namespace Domain.Contracts.Requests
{
    public record EstoqueMovimentoRequest
    {
        public long UnidadeId { get; init; }
        public long ProdutoId { get; init; }
        public string TipoMovimento { get; init; }
        public int Quantidade { get; init; }
        public string Motivo { get; init; }
    }
}

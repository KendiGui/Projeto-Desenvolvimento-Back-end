namespace Domain.Contracts.Responses
{
    public record EstoqueResponse
    {
        public long EstoqueId { get; init; }
        public long UnidadeId { get; init; }
        public long ProdutoId { get; init; }
        public string ProdutoNome { get; init; } = string.Empty;
        public int QuantidadeAtual { get; init; }
        public int QuantidadeMinima { get; init; }
        public bool EstoqueBaixo => QuantidadeAtual <= QuantidadeMinima;
    }
}

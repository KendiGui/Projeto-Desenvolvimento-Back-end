namespace Domain.Contracts.Responses
{
    public record ProdutoResponse
    {
        public long Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public string Descricao { get; init; } = string.Empty;
        public decimal Preco { get; init; }
        public string Categoria { get; init; } = string.Empty;
        public bool Ativo { get; init; }
        public bool Sazonal { get; init; }
    }
}

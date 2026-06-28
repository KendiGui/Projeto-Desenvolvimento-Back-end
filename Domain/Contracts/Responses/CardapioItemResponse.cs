namespace Domain.Contracts.Responses
{
    public record CardapioItemResponse
    {
        public long ProdutoId { get; init; }
        public string Nome { get; init; } = string.Empty;
        public string Descricao { get; init; } = string.Empty;
        public string Categoria { get; init; } = string.Empty;
        public decimal Preco { get; init; }
        public bool Disponivel { get; init; }
        public bool Sazonal { get; init; }
    }
}

namespace Domain.Contracts.Requests
{
    public record ProdutoRequest
    {
        public string Nome { get; init; }
        public string Descricao { get; init; }
        public decimal Preco { get; init; }
        public string Categoria { get; init; }
        public bool Ativo { get; init; } = true;
        public bool Sazonal { get; init; } = false;
    }
}

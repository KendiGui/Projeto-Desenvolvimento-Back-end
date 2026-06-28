namespace Domain.Contracts.Responses
{
    public record MovimentoEstoqueResponse
    {
        public long Id { get; init; }
        public long EstoqueId { get; init; }
        public long UnidadeId { get; init; }
        public long ProdutoId { get; init; }
        public string TipoMovimento { get; init; } = string.Empty;
        public int Quantidade { get; init; }
        public string Motivo { get; init; } = string.Empty;
        public long? UsuarioId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

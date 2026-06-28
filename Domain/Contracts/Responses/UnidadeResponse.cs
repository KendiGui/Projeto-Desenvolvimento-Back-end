namespace Domain.Contracts.Responses
{
    public record UnidadeResponse
    {
        public long Id { get; init; }
        public string Nome { get; init; } = string.Empty;
        public string Cidade { get; init; } = string.Empty;
        public string Estado { get; init; } = string.Empty;
        public string Endereco { get; init; } = string.Empty;
        public bool Ativa { get; init; }
    }
}

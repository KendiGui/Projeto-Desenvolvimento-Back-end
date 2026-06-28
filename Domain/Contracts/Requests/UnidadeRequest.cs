namespace Domain.Contracts.Requests
{
    public record UnidadeRequest
    {
        public string Nome { get; init; }
        public string Cidade { get; init; }
        public string Estado { get; init; }
        public string Endereco { get; init; }
        public bool Ativa { get; init; } = true;
    }
}

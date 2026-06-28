namespace Domain.Contracts.Responses
{
    public record AuditoriaResponse
    {
        public long Id { get; init; }
        public long? UsuarioId { get; init; }
        public string Acao { get; init; } = string.Empty;
        public string Entidade { get; init; } = string.Empty;
        public long? EntidadeId { get; init; }
        public string? DadosAntes { get; init; }
        public string? DadosDepois { get; init; }
        public string? Ip { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

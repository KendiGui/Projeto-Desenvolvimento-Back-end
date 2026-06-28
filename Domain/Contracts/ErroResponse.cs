namespace Domain.Contracts
{
    public record ErroDetalhe(string Field, string Issue);

    public record ErroResponse(string Erro, string Mensagem)
    {
        public IEnumerable<ErroDetalhe>? Detalhes { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
        public string? Path { get; init; }
        public string? RequestId { get; init; }
    }
}

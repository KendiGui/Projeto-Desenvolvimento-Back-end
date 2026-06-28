namespace Domain.Contracts.Responses
{
    public record FidelidadeSaldoResponse
    {
        public long ClienteId { get; init; }
        public int Pontos { get; init; }
        public bool ConsentimentoMarketing { get; init; }
        public DateTime? ConsentimentoData { get; init; }
    }
}

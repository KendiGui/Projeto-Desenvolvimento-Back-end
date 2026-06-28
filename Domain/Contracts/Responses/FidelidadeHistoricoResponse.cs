namespace Domain.Contracts.Responses
{
    public record FidelidadeHistoricoResponse
    {
        public long Id { get; init; }
        public string Tipo { get; init; } = string.Empty;
        public int Pontos { get; init; }
        public string Descricao { get; init; } = string.Empty;
        public long? PedidoId { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}

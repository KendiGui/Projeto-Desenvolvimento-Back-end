namespace Domain.Contracts.Requests
{
    public record ResgatarPontosRequest
    {
        public int Pontos { get; init; }
        public string Descricao { get; init; } = "Resgate de pontos";
    }
}

namespace Domain.Contracts.Requests
{
    public record CardapioProdutoRequest
    {
        public long ProdutoId { get; init; }
        public bool Disponivel { get; init; } = true;
        public decimal? PrecoCustomizado { get; init; }
    }
}

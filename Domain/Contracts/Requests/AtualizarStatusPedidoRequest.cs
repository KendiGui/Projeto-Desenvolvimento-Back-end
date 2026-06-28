namespace Domain.Contracts.Requests
{
    public record AtualizarStatusPedidoRequest
    {
        public string NovoStatus { get; init; }
    }
}

using Domain.Enums;

namespace Service.Gateways
{
    public class PaymentResult
    {
        public bool Aprovado { get; init; }
        public StatusPagamentoEnum Status { get; init; }
        public string TransactionId { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string PayloadRequest { get; init; } = string.Empty;
        public string PayloadResponse { get; init; } = string.Empty;
    }
}

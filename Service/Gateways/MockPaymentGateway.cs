using System.Text.Json;
using Domain.Entities;
using Domain.Enums;
using Service.Interfaces;

namespace Service.Gateways
{
    public class MockPaymentGateway : IPaymentGateway
    {
        public Task<PaymentResult> ProcessarPagamentoAsync(Pedido pedido, string formaPagamento)
        {
            formaPagamento = (formaPagamento ?? string.Empty).Trim().ToUpperInvariant();

            var payloadRequest = JsonSerializer.Serialize(new
            {
                pedidoId = pedido.Id,
                total = pedido.Total,
                formaPagamento
            });

            var transactionId = $"mock-{Guid.NewGuid():N}".Substring(0, 13);

            bool aprovado = formaPagamento == "MOCK" && pedido.Total > 0;

            string message = aprovado
                ? "Pagamento aprovado pelo mock."
                : pedido.Total <= 0
                    ? "Pagamento recusado: total inválido."
                    : "Pagamento recusado pelo mock.";

            var status = aprovado ? StatusPagamentoEnum.Aprovado : StatusPagamentoEnum.Recusado;

            var payloadResponse = JsonSerializer.Serialize(new
            {
                status = status.ToString(),
                transactionId,
                message
            });

            var result = new PaymentResult
            {
                Aprovado = aprovado,
                Status = status,
                TransactionId = transactionId,
                Message = message,
                PayloadRequest = payloadRequest,
                PayloadResponse = payloadResponse
            };

            return Task.FromResult(result);
        }
    }
}

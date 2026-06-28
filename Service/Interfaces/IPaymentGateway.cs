using Domain.Entities;
using Service.Gateways;

namespace Service.Interfaces
{
    public interface IPaymentGateway
    {
        Task<PaymentResult> ProcessarPagamentoAsync(Pedido pedido, string formaPagamento);
    }
}

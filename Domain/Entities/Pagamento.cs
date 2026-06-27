using Domain.Enums;

namespace Domain.Entities
{
    public class Pagamento : BaseEntity
    {
        public long PedidoId { get; set; }
        public StatusPagamentoEnum Status { get; set; }
        public string FormaPagamento { get; set; }
        public string GatewayTransactionId { get; set; }
        public string PayloadRequest { get; set; }
        public string PayloadResponse { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}

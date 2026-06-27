namespace Domain.Entities
{
    public class Pagamento : BaseEntity
    {
        public long PedidoId { get; set; }
        public string Status { get; set; } // PENDENTE, APROVADO, RECUSADO, ERRO
        public string FormaPagamento { get; set; }
        public string GatewayTransactionId { get; set; }
        public string PayloadRequest { get; set; }
        public string PayloadResponse { get; set; }

        public virtual Pedido Pedido { get; set; }
    }
}

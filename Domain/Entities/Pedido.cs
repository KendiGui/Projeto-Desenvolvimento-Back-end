using Domain.Enums;

namespace Domain.Entities
{
    public class Pedido : BaseEntity
    {
        public long ClienteId { get; set; }
        public long UnidadeId { get; set; }
        public CanalPedidoEnum CanalPedido { get; set; }
        public StatusPedidoEnum Status { get; set; }
        public decimal Total { get; set; }

        public virtual Usuario Cliente { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public virtual Pagamento Pagamento { get; set; }
    }
}

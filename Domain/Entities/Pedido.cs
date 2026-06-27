namespace Domain.Entities
{
    public class Pedido : BaseEntity
    {
        public long ClienteId { get; set; }
        public long UnidadeId { get; set; }
        public string CanalPedido { get; set; } // APP, TOTEM, BALCAO, PICKUP, WEB
        public string Status { get; set; } // CRIADO, AGUARDANDO_PAGAMENTO, PAGO, etc
        public decimal Total { get; set; }

        public virtual Usuario Cliente { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
        public virtual Pagamento Pagamento { get; set; }
    }
}

namespace Domain.Entities
{
    public class ItemPedido : BaseEntity
    {
        public long PedidoId { get; set; }
        public long ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal Subtotal { get; set; }

        public virtual Pedido Pedido { get; set; }
        public virtual Produto Produto { get; set; }
    }
}

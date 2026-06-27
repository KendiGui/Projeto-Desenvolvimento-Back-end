namespace Domain.Entities
{
    public class Produto : BaseEntity
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; }
        public bool Ativo { get; set; }
        public bool Sazonal { get; set; }

        public virtual ICollection<UnidadeProduto> UnidadeProdutos { get; set; } = new List<UnidadeProduto>();
        public virtual ICollection<Estoque> Estoques { get; set; } = new List<Estoque>();
        public virtual ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();
    }
}

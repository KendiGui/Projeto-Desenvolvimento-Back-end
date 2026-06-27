namespace Domain.Entities
{
    public class Unidade : BaseEntity
    {
        public string Nome { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Endereco { get; set; }
        public bool Ativa { get; set; }

        public virtual ICollection<UnidadeProduto> UnidadeProdutos { get; set; } = new List<UnidadeProduto>();
        public virtual ICollection<Estoque> Estoques { get; set; } = new List<Estoque>();
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}

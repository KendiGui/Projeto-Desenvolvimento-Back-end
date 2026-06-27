namespace Domain.Entities
{
    public class Estoque : BaseEntity
    {
        public long UnidadeId { get; set; }
        public long ProdutoId { get; set; }
        public int QuantidadeAtual { get; set; }
        public int QuantidadeMinima { get; set; }

        public virtual Unidade Unidade { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual ICollection<MovimentoEstoque> Movimentos { get; set; } = new List<MovimentoEstoque>();
    }
}

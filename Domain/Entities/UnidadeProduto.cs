namespace Domain.Entities
{
    public class UnidadeProduto : BaseEntity
    {
        public long UnidadeId { get; set; }
        public long ProdutoId { get; set; }
        public bool Disponivel { get; set; }
        public decimal? PrecoCustomizado { get; set; }

        public virtual Unidade Unidade { get; set; }
        public virtual Produto Produto { get; set; }
    }
}

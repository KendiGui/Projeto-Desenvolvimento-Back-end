namespace Domain.Entities
{
    public class MovimentoEstoque : BaseEntity
    {
        public long EstoqueId { get; set; }
        public string TipoMovimento { get; set; } // ENTRADA, SAIDA, AJUSTE
        public int Quantidade { get; set; }
        public string Motivo { get; set; }
        public long? UsuarioId { get; set; }

        public virtual Estoque Estoque { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}

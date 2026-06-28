namespace Domain.Entities
{
    public class Auditoria : BaseEntity
    {
        public long? UsuarioId { get; set; }
        public string Acao { get; set; }
        public string Entidade { get; set; }
        public long? EntidadeId { get; set; }
        public string DadosAntes { get; set; }
        public string DadosDepois { get; set; }
        public string Ip { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}

namespace Domain.Entities
{
    public class FidelidadeConta : BaseEntity
    {
        public long ClienteId { get; set; }
        public int Pontos { get; set; }
        public bool ConsentimentoMarketing { get; set; }
        public DateTime? ConsentimentoData { get; set; }

        public virtual Usuario Cliente { get; set; }
        public virtual ICollection<FidelidadeHistorico> Historicos { get; set; } = new List<FidelidadeHistorico>();
    }
}

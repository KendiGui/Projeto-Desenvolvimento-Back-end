using Domain.Enums;

namespace Domain.Entities
{
    public class FidelidadeHistorico : BaseEntity
    {
        public long ClienteId { get; set; }
        public long? PedidoId { get; set; }
        public TipoFidelidadeEnum Tipo { get; set; }
        public int Pontos { get; set; }
        public string Descricao { get; set; }

        public virtual Usuario Cliente { get; set; }
        public virtual Pedido Pedido { get; set; }
    }
}

namespace Domain.Contracts.Exceptions
{
    public class EstoqueInsuficienteException(string mensagem, IEnumerable<ErroDetalhe>? detalhes = null) : BusinessException("ESTOQUE_INSUFICIENTE", mensagem, 409, detalhes)
    {
    }
}

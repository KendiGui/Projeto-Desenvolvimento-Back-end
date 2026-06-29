namespace Domain.Contracts.Exceptions
{
    public class EmptyListException(string mensagem, IEnumerable<ErroDetalhe>? detalhes = null) : BusinessException("SEM_CONTEUDO", mensagem, 204, detalhes)
    {
    }
}

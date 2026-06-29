namespace Domain.Contracts.Exceptions
{
    public class NotFoundException(string mensagem, IEnumerable<ErroDetalhe>? detalhes = null) : BusinessException("NOT_FOUND", mensagem, 404, detalhes)
    {
    }
}

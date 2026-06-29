namespace Domain.Contracts.Exceptions
{
    public class ValidationException(string mensagem, IEnumerable<ErroDetalhe>? detalhes = null) : BusinessException("VALIDATION_ERROR", mensagem, 422, detalhes)
    {
    }
}

namespace Domain.Contracts.Exceptions
{
    /// <summary>
    /// Erro de validação de entrada (HTTP 422).
    /// </summary>
    public class ValidationException : BusinessException
    {
        public ValidationException(string mensagem, IEnumerable<ErroDetalhe>? detalhes = null)
            : base("VALIDATION_ERROR", mensagem, 422, detalhes)
        {
        }
    }
}

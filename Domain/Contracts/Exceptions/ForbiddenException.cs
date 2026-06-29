namespace Domain.Contracts.Exceptions
{
    public class ForbiddenException(string mensagem) : BusinessException("FORBIDDEN", mensagem, 403)
    {
    }
}

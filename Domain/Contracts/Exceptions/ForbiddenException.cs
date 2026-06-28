namespace Domain.Contracts.Exceptions
{
    /// <summary>
    /// Acesso negado por regra de negócio/perfil (HTTP 403).
    /// </summary>
    public class ForbiddenException : BusinessException
    {
        public ForbiddenException(string mensagem)
            : base("FORBIDDEN", mensagem, 403)
        {
        }
    }
}

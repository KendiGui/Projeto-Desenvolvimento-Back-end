namespace Domain.Contracts.Exceptions
{
    /// <summary>
    /// Exceção base para regras de negócio. Carrega um código de erro e o
    /// status HTTP correspondente para que o middleware padronize a resposta.
    /// </summary>
    public class BusinessException : Exception
    {
        public string Codigo { get; }
        public int StatusCode { get; }
        public IEnumerable<ErroDetalhe>? Detalhes { get; }

        public BusinessException(string codigo, string mensagem, int statusCode = 400, IEnumerable<ErroDetalhe>? detalhes = null)
            : base(mensagem)
        {
            Codigo = codigo;
            StatusCode = statusCode;
            Detalhes = detalhes;
        }
    }
}

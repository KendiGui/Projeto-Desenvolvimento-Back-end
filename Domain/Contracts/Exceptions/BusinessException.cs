namespace Domain.Contracts.Exceptions
{
    public class BusinessException(string codigo, string mensagem, int statusCode = 400, IEnumerable<ErroDetalhe>? detalhes = null) : Exception(mensagem)
    {
        public string Codigo { get; } = codigo;
        public int StatusCode { get; } = statusCode;
        public IEnumerable<ErroDetalhe>? Detalhes { get; } = detalhes;
    }
}

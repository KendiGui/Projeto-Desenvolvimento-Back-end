namespace Domain.Contracts.Exceptions
{
    public class PagamentoRecusadoException(string mensagem) : BusinessException("PAGAMENTO_RECUSADO", mensagem, 402)
    {
    }
}

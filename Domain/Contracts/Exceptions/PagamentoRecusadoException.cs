namespace Domain.Contracts.Exceptions
{
    /// <summary>
    /// Pagamento recusado pelo gateway (HTTP 402).
    /// </summary>
    public class PagamentoRecusadoException(string mensagem) : BusinessException("PAGAMENTO_RECUSADO", mensagem, 402)
        {
        }
    }

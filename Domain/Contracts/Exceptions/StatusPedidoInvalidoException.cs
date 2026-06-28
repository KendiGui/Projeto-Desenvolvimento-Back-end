namespace Domain.Contracts.Exceptions
{
    public class StatusPedidoInvalidoException(string mensagem) : BusinessException("STATUS_PEDIDO_INVALIDO", mensagem, 409)
    {
    }
}

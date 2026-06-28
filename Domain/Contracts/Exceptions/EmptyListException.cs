namespace Domain.Contracts.Exceptions
{
    public class EmptyListException : Exception
    {
        public EmptyListException(string message)
        : base(message)
        {
        }
        public EmptyListException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

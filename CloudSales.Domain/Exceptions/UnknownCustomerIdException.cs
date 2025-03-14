namespace CloudSales.Domain.Exceptions
{
    public class UnknownCustomerIdException : Exception
    {
        public UnknownCustomerIdException() : base() { }

        public UnknownCustomerIdException(string message) : base(message) { }
    }
}

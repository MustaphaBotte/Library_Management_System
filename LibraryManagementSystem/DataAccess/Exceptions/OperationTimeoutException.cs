namespace LMS.DataAccess.Exceptions
{
    public class OperationTimeoutException : Exception
    {
        public OperationTimeoutException(string message) : base(message) { }
        public OperationTimeoutException(string message, Exception inner) : base(message, inner) { }
    }

}

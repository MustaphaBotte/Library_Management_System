namespace LMS.DataAccess.Exceptions
{
    public class CannotInsertNullException : Exception
    {
        public CannotInsertNullException(string message) : base(message) { }
        public CannotInsertNullException(string message, Exception inner) : base(message, inner) { }
    }

}

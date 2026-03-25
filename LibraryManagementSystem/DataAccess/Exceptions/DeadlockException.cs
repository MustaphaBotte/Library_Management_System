namespace LMS.DataAccess.Exceptions
{
    public class DeadlockException : Exception
    {
        public DeadlockException(string message) : base(message) { }
        public DeadlockException(string message, Exception inner) : base(message, inner) { }
    }

}

namespace LMS.DataAccess.Exceptions
{
    public class ForeignKeyViolationException : Exception
    {
        public ForeignKeyViolationException(string message) : base(message) { }
        public ForeignKeyViolationException(string message, Exception inner) : base(message, inner) { }
    }



}

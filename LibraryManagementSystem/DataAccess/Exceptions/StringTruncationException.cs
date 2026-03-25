namespace LMS.DataAccess.Exceptions
{
    public class StringTruncationException : Exception
    {
        public StringTruncationException(string message) : base(message) { }
        public StringTruncationException(string message, Exception inner) : base(message, inner) { }
    }


}

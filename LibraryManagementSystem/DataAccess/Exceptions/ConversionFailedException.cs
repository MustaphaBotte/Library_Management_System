namespace LMS.DataAccess.Exceptions
{
    public class ConversionFailedException : Exception
    {
        public ConversionFailedException(string message) : base(message) { }
        public ConversionFailedException(string message, Exception inner) : base(message, inner) { }
    }


}

namespace LMS.DataAccess.Exceptions
{
    public class UniqueConstraintViolation : Exception
    {
        public UniqueConstraintViolation(string message) : base(message) { }
        public UniqueConstraintViolation(string message, Exception inner) : base(message, inner) { }
    }
}

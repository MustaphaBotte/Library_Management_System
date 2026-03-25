namespace LMS.DataAccess.Exceptions
{
    public enum EnSqlErrorCode
    {
        // SQL Server errors
        UniqueConstraintViolation = 2627,
        DuplicateKey = 2601,
        ForeignKeyViolation = 547,
        CannotInsertNull = 515,
        StringTruncation = 8152,
        ConversionFailed = 245,
        Deadlock = 1205,

        // ADO.NET client-side
        Timeout = -2
    }
}

using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace LMS.DataAccess
{
    public class Exceptions
    {

        public class SqlExceptionHandler
        {

            [DoesNotReturn]
            public static void Handle(SqlException e)
            {
                 if(e.Number == (int)EnSqlErrorCode.UniqueConstraintViolation ||
                        e.Number == (int)EnSqlErrorCode.DuplicateKey)
                 {
                    throw new UniqueConstraintViolation("Unique Constraint Violation.", e);
                 };

                 if(e.Number == (int)EnSqlErrorCode.CannotInsertNull)
                 {
                    throw new CannotInsertNullException("A required field was not provided.", e);
                 }

                 if(e.Number == (int)EnSqlErrorCode.ForeignKeyViolation)
                 {
                    throw new ForeignKeyViolationException("Referential integrity violation.", e);
                 }

                 if(e.Number == (int)EnSqlErrorCode.StringTruncation)
                 {
                    throw new StringTruncationException("A value exceeds the maximum allowed length for its field.", e);
                 }

                 if(e.Number == (int)EnSqlErrorCode.Deadlock)
                 {
                    throw new DeadlockException("The operation failed due to a database deadlock. Please try again.", e);
                 }

                 if(e.Number == (int)EnSqlErrorCode.Timeout)
                 {
                    throw new TimeoutException("The database did not respond in time. Please try again later.", e);
                 }
                throw new Exception("An unexpected error occurred. Please contact support.",e);
            }

        }

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

        public class UniqueConstraintViolation : Exception
        {
            string ColumnName = "";
            public UniqueConstraintViolation(string message) : base(message) { }
            public UniqueConstraintViolation(string message, Exception inner) : base(message, inner) { }
        }

        public class ForeignKeyViolationException : Exception
        {
            public ForeignKeyViolationException(string message) : base(message) { }
            public ForeignKeyViolationException(string message, Exception inner) : base(message, inner) { }
        }

        public class CannotInsertNullException : Exception
        {
            public CannotInsertNullException(string message) : base(message) { }
            public CannotInsertNullException(string message, Exception inner) : base(message, inner) { }
        }

        public class StringTruncationException : Exception
        {
            public StringTruncationException(string message) : base(message) { }
            public StringTruncationException(string message, Exception inner) : base(message, inner) { }
        }

        public class ConversionFailedException : Exception
        {
            public ConversionFailedException(string message) : base(message) { }
            public ConversionFailedException(string message, Exception inner) : base(message, inner) { }
        }

        public class DeadlockException : Exception
        {
            public DeadlockException(string message) : base(message) { }
            public DeadlockException(string message, Exception inner) : base(message, inner) { }
        }

        public class OperationTimeoutException : Exception
        {
            public OperationTimeoutException(string message) : base(message) { }
            public OperationTimeoutException(string message, Exception inner) : base(message, inner) { }
        }
    }
}




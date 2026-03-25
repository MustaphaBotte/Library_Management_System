using Microsoft.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace LMS.DataAccess.Exceptions
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

     

       

      
     

       
      

        
}




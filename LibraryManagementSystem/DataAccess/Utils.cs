

namespace LMS.DataAccess
{
    public class Utils
    {
        public static bool IsNullOrDBNull(object param)
        {
            return (param == null || param == DBNull.Value);
        }
    }
}

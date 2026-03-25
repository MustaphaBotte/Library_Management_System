namespace LMS.DataAccess.Utils
{
    public class DbUtils
    {
        public static bool IsNullOrDBNull(object param)
        {
            return param == null || param == DBNull.Value;
        }
    }
}

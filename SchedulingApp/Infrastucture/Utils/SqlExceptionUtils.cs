using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;

namespace SchedulingApp.Infrastucture.Utils
{
    [ExcludeFromCodeCoverage]
    public static class SqlExceptionUtils
    {
        public static bool IsAnyOfUniqueKeyViolationsError(this SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 2627:
                case 547:
                case 2601:
                    return true;
                default:
                    return false;
            }
        }
    }
}

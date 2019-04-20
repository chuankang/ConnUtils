using FluentData;

namespace FxConsoleApp.ORM
{
    public class FluentDataSqlHelper
    {
        public static IDbContext Context()
        {
            return new DbContext().ConnectionStringName("OwnCon", new SqlServerProvider());
        }
    }
}

using System.Configuration;
using System.Data.SqlClient;

namespace FxConsoleApp.Utils
{
    public class ConfigUtil
    {
        public static SqlConnection GetOwnCon()
        {
            var con = new SqlConnection(ConfigurationManager.ConnectionStrings["OwnCon"].ConnectionString);
            con.Open();
            return con;
        }
    }
}

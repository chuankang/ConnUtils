using Dapper;
using FxConsoleApp.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FxConsoleApp.ORM
{
    public class DapperSqlHelper
    {
        public static List<T> GetList<T>(string query, object param = null)
        {
            using (var conn = ConfigUtil.GetOwnCon())
            {
                return conn.Query<T>(query, param).ToList();
            }
        }

        public static DataTable GetDateTable(string sql, object param = null)
        {
            using (var conn = ConfigUtil.GetOwnCon())
            {
                var dt = new DataTable();
                var reader = conn.ExecuteReader(sql);
                dt.Load(reader);
                return dt;
            }
        }

        public static DataSet GetDataSet(string sql, params SqlParameter[] sqlParameters)
        {
            var ds = new DataSet();
            using (var conn = ConfigUtil.GetOwnCon())
            {
                var cmd = new SqlCommand(sql, conn);
                if (sqlParameters != null)
                {
                    foreach (var parameter in sqlParameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            return ds;
        }

        public static int Execute(string sql, object param = null)
        {
            using (var conn = ConfigUtil.GetOwnCon())
            {
                return conn.Execute(sql, param);
            }
        }
    }
}

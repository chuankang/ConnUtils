using System.Data;
using System.IO;
using System.Text;

namespace ConnUtils
{
    public static class CsvHelper
    {
        /// <summary>
        /// 导出报表为Csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">物理路径</param>
        /// <param name="tableHeader">表头</param>
        /// <param name="columName">字段标题,逗号分隔</param>
        public static bool DataTableToCsv(DataTable dt, string filePath, string tableHeader, string columName)
        {
            try
            {
                var strmWriterObj = new StreamWriter(filePath, false, Encoding.UTF8);
                strmWriterObj.WriteLine(tableHeader);
                strmWriterObj.WriteLine(columName);
                for (var i = 0; i < dt.Rows.Count; i++)
                {
                    var strBufferLine = "";
                    for (var j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j > 0)
                            strBufferLine += ",";
                        strBufferLine += dt.Rows[i][j].ToString();
                    }
                    strmWriterObj.WriteLine(strBufferLine);
                }
                strmWriterObj.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 将Csv读入DataTable
        /// </summary>
        /// <param name="filePath">csv文件路径</param>
        /// <param name="n">表示第n行是字段title,第n+1行是记录开始</param>
        public static DataTable CsvToDataTable(string filePath, int n)
        {
            var dt = new DataTable();
            var reader = new StreamReader(filePath, Encoding.UTF8, false);
            int m = 0;
            while (reader.Peek() > 0)
            {
                m = m + 1;
                string str = reader.ReadLine();
                if (m >= n + 1)
                {
                    var split = str.Split(',');

                    DataRow dr = dt.NewRow();
                    int i;
                    for (i = 0; i < split.Length; i++)
                    {
                        dr[i] = split[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}

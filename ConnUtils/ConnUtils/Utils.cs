using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ConnUtils
{
    public class Utils
    {
        /// <summary>
        /// List转DataTable
        /// </summary>
        public static DataTable ListToTable<T>(IList<T> list)
        {
            var type = typeof(T);
            var dt = new DataTable();
            var propertyInfoList = type.GetProperties(BindingFlags.DeclaredOnly 
                                                      | BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in propertyInfoList)
            {
                dt.Columns.Add(property.Name);
            }
            foreach (var model in list)
            {
                var row = dt.NewRow();
                dt.Rows.Add(row);
                foreach (var property in propertyInfoList)
                {
                    row[property.Name] = property.GetValue(model, null);
                }
            }
            
            return dt;
        }

        /// <summary>
        /// 时间戳转时间
        /// </summary>
        public DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        public int DateTimeToStamp(DateTime time)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encode">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string result)
        {
            string decode;
            var bytes = Convert.FromBase64String(result);
            try
            {
                decode = encode.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encodeType">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string Base64Encode(Encoding encodeType, string source)
        {
            string encode;
            byte[] bytes = encodeType.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }
    }
}

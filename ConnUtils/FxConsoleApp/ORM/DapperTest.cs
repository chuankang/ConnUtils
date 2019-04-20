using System;
using System.Diagnostics;

namespace FxConsoleApp.ORM
{
    public class DapperTest : IWork
    {
        public void DoWork()
        {
            Insert();
        }

        public static void Insert()
        {
            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 10000; i++)
            {
                var sql = $@"
                INSERT INTO dbo.Student
                         ( Name ,
                           Age ,
                           Sex ,
                           Birthday ,
                           Address
                         )
                 VALUES  ( N'james' , -- Name - nvarchar(20)
                           {i}, -- Age - int
                           N'男' , -- Sex - nvarchar(5)
                           GETDATE()+{i} ,
                           'Dapper'
                         ) ";
               DapperSqlHelper.Execute(sql);
            }

            sw.Stop();

            Console.WriteLine($"Dapper插入10000条耗时:{sw.ElapsedMilliseconds}毫秒");

        }
    }
}

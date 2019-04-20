using System;
using System.Diagnostics;

namespace FxConsoleApp.ORM
{
    public class FluentDataTest : IWork
    {
        public void DoWork()
        {
            Insert();
        }

        public static void Insert()
        {
            var context = FluentDataSqlHelper.Context();

            var sw = new Stopwatch();
            sw.Start();

            for (var i = 0; i < 10000; i++)
            {
                var sql = $@"
                INSERT INTO dbo.Student
                         ( Name ,
                           Age ,
                           Sex ,
                           Birthday
                         )
                 VALUES  ( N'james' , -- Name - nvarchar(20)
                           {i}, -- Age - int
                           N'男' , -- Sex - nvarchar(5)
                           GETDATE()+{i}
                         ) ";
                context.Sql(sql);
            }

            sw.Stop();

            Console.WriteLine($"FluentData插入10000条耗时:{sw.ElapsedMilliseconds}毫秒");
        }
        
    }
}

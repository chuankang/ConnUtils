using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using FluentData;

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

            for (var i = 0; i < 1000; i++)
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

            Console.WriteLine($"FluentData插入1000条耗时:{sw.ElapsedMilliseconds}毫秒");
        }
        
    }
}

using System;
using System.Diagnostics;

namespace FxConsoleApp.ORM
{
    public class PetapocoTest : IWork
    {
        public void DoWork()
        {
            Insert();
        }

        public static void Insert()
        {
            var db = new PetaPoco.Database("OwnCon");

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
                db.Execute(sql);
            }

            sw.Stop();

            Console.WriteLine($"Petapoco插入10000条耗时:{sw.ElapsedMilliseconds}毫秒");
        }
    }
}

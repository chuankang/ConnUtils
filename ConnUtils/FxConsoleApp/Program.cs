using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FxConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestException();
            Console.ReadLine();
        }

        #region 测试装箱

        private static void TestBoxing()
        {
            var sw = new Stopwatch();

            sw.Start();
            for (var i = 0; i < 10_000_000; i++)
            {
                string x = "abc" + i;
            }
            sw.Stop();

            Console.WriteLine($"Fx拼接加值类型用时:{sw.ElapsedMilliseconds}毫秒");//1650

            var sw2 = new Stopwatch();

            sw2.Start();
            for (var i = 0; i < 10_000_000; i++)
            {
                string x2 = "abc" + $"{i}";
            }
            sw2.Stop();

            Console.WriteLine($"Fx拼接字符串用时:{sw2.ElapsedMilliseconds}毫秒");//2332

            Console.ReadLine();
        }

        #endregion

        #region 测试异常

        private static void TestException()
        {
            var sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    throw new Exception("test");
                }
                catch { }
            }


            sw.Stop();

            Console.WriteLine($"FX调用栈抛异常:{sw.ElapsedMilliseconds}毫秒");//1341


            var sw2 = new Stopwatch();
            sw2.Start();
            for (int i = 0; i < 100; i++)
            {
                try
                {
                    System.IO.File.OpenRead("c:\\不存在的txt.txt");
                }
                catch { }
            }
            sw2.Stop();
            Console.WriteLine($"FX调用栈的深入抛异常:{sw2.ElapsedMilliseconds}毫秒");//1341


            Console.ReadLine();
        }

        #endregion
    }
}

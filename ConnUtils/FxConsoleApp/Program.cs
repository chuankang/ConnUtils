using Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FxConsoleApp.ORM;

namespace FxConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" ");


            Console.ReadLine();
        }

        private static void Begin()
        {
            var x = new DapperTest();
            x.DoWork();
            var y = new FluentDataTest();
            y.DoWork();
            var z = new PetapocoTest();
            z.DoWork();

            //IWork work;

            //var serverType = System.Configuration.ConfigurationManager.AppSettings["serverType"];
            //switch (serverType)
            //{
            //    case "DapperTest":
            //        work = new DapperTest();
            //        work.DoWork();;
            //        break;

            //    case "PetapocoTest":
            //        work = new PetapocoTest();
            //        work.DoWork();
            //        break;

            //    case "FluentDataTest":
            //        work = new FluentDataTest();
            //        work.DoWork();
            //        break;
            //}
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
            Console.WriteLine($"FX调用栈抛异常:{sw.ElapsedMilliseconds}毫秒");//590

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
            Console.WriteLine($"FX调用栈的深入抛异常:{sw2.ElapsedMilliseconds}毫秒");//597

            Console.ReadLine();
        }

        #endregion

        #region 测试线程安全

        private static void TestThreadSafe()
        {
            //线程不安全
            var list = new List<Student>();
            //线程安全
            var bag = new ConcurrentBag<Student>();

            Parallel.For(0, 100000, i =>
            {
                list.Add(new Student { Age = i });
                bag.Add(new Student { Age = i });
                //10w数据分布在不同的线程下执行
                Console.Write(Thread.CurrentThread.ManagedThreadId);
            });
            Console.WriteLine("==============");
            Console.WriteLine(list.Count);
            Console.WriteLine(bag.Count);
        }

        #endregion

        #region test

        //test3

        #endregion
    }
}

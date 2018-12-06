using System;
using System.Collections.Generic;
using ConnUtils;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<int>();
            Utils.ListToTable(list);
            Console.WriteLine();
        }
    }
}

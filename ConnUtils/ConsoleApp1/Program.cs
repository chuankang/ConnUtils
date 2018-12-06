using System;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var studentList = new List<Student>
            {
                new Student {Name = "吴明军", Age = 40},
                new Student {Name = "孙传康", Age = 20},
            };

            foreach (var student in studentList)
            {
                if (student.Name == "吴明军")
                {
                    student.Name = "张恒";
                }
            }

            Console.WriteLine($"{studentList[0].Name},{studentList[1].Name}");

            Console.ReadLine();
        }
    }

    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

}

using System;

namespace Models
{
    public class Student
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
}

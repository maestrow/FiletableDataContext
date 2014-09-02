using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo;
using Demo.Domain;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string path;
            var test = new TestActions();
            path = test.CreateDir("ccc");
            test.CreateFile("hello ccc.txt", path);
            path = test.CreateDir("ddd", path);
            test.CreateFile("hello ddd.txt", path);
        }
    }
}

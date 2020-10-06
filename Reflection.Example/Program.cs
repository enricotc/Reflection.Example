using System;
using Reflection.Example.Shared;

namespace Reflection.Example
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Class1 obj = new Class1();
            Console.WriteLine($"Hello World! Il mio valore è: {obj.Licenza}");
            int i = 0;
            Console.ReadLine();
        }
    }
}

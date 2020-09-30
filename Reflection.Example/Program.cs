using System;
using Reflection.Example.Shared;

namespace Reflection.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Class1 obj = new Class1();
            Console.WriteLine($"Hello World! Il mio valore è: {obj.Licenza}");
            Console.ReadLine();
        }
    }
}

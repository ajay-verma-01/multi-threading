using System;
using System.Collections.Generic;

namespace Yield
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            foreach (var item in GenerateNumbers())
            {
                Console.WriteLine(item);
            }
        }


        private static IEnumerable<int> GenerateNumbers()
        {
            for (int i = 0; i < 10; i++)
            {
                yield return i + 10;
            }
        }
    }
}

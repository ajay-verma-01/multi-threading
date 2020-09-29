using System;
using System.Threading;
namespace Volatile
{
    class Program
    {
        static string result;
        static volatile bool done;
        static void SetVolatile()
        {
            result = "Csharpcorner.com";
            done = true;
        }
        static void Main(string[] args)
        {
            new Thread(new ThreadStart(SetVolatile)).Start();
            Thread.Sleep(200);
            if (done)
            {
                Console.WriteLine(result);
            }
            Console.Read();
        }
    }
}
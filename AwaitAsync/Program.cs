using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace AwaitAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private void DumpWebPage(string uri)
        {
            WebClient wc = new WebClient();
            string page = wc.DownloadString(uri);
            Console.WriteLine(page);
        }

        private async void DumpWebPageAsync(string uri)
        {
            WebClient wc = new WebClient();
            string page = await wc.DownloadStringTaskAsync(uri);
            //Task<string> DownloadStringTaskAsync(string address)
            Console.WriteLine(page);
        }

        private void DumpWebPageTaskBased(string uri)
        {
            WebClient webClient = new WebClient();
            Task<string> task = webClient.DownloadStringTaskAsync(uri);
            task.ContinueWith(t => { Console.WriteLine(t.Result); });
        }


        public async void Test()
        {
            Task operation1 = Operation1();
            Task operation2 = Operation2();
            await operation1;
            await operation2;

        }

        private Task Operation()
        {
            return Task.Delay(TimeSpan.FromMilliseconds(500));
        }

        private Task Operation1()
        {
            return Task.Delay(TimeSpan.FromMilliseconds(500));
        }

        private Task Operation2()
        {
            return Task.Delay(TimeSpan.FromMilliseconds(1000));
        }


     
        private static async void CatchMultipleExceptionsWithAwait()
        {
            int[] numbers = { 0 };

            Task<int> t1 = Task.Run(() => 5 / numbers[0]);
            Task<int> t2 = Task.Run(() => numbers[1]);

            Task<int[]> allTask = Task.WhenAll(t1, t2);
            try
            {
                await allTask;
            }
            catch
            {
                foreach (var ex in allTask.Exception.InnerExceptions)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        static async Task Catcher()
        {
            try
            {
                Task thrower = Thrower();
                await thrower;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex);
            }
        }

        async static Task Thrower()
        {
            await Task.Delay(100);
            throw new InvalidOperationException();
        }

        private static void AttachedToParent()
        {
            Task.Factory.StartNew(() =>
            {
                Task nested = Task.Factory.StartNew(() =>
                    Console.WriteLine("hello world"), TaskCreationOptions.AttachedToParent);
            }).Wait();

            Thread.Sleep(100);
        }

        private static int Print(bool isEven, CancellationToken token)
        {
            Console.WriteLine($"Is thread pool thread:{Thread.CurrentThread.IsThreadPoolThread}");
            int total = 0;
            if (isEven)
            {
                for (int i = 0; i < 100; i += 2)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id = {Task.CurrentId}. Value={i}");
                }
            }
            else
            {
                for (int i = 1; i < 100; i += 2)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancellation Requested");
                    }
                    token.ThrowIfCancellationRequested();
                    total++;
                    Console.WriteLine($"Current task id = {Task.CurrentId}. Value={i}");
                }
            }

            return total;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleAppProducerConsumer
{
    class Program
    {
        static object baton = new object();
        static Queue<int> produckt = new Queue<int>();
        static Random random = new Random();
        static void Main(string[] args)
        {
            Thread threadSetProdukt = new Thread(SetProduckt);
            Thread threadGetProdukt = new Thread(GetProduckt);

            threadSetProdukt.Start();
            threadGetProdukt.Start();

            Console.ReadKey();
        }

        static void GetProduckt()
        {
            while (true)
            {
                Monitor.Enter(baton);
                try
                {
                    while (produckt.Count == 0)
                    {
                        Console.WriteLine("custumer is waiting for the baton...");
                        Monitor.Wait(baton);
                    }

                    Console.WriteLine("Custumer gat the baton");
                    while (produckt.Count != 0)
                    {
                        Console.WriteLine("produckt dequeue : " + produckt.Dequeue());
                        Thread.Sleep(500);

                    }
                }
                finally
                {
                    Monitor.Exit(baton);
                }
            }
        }

        static void SetProduckt()
        {
            while (true)
            {
                Monitor.Enter(baton);
                try
                {
                    Console.WriteLine("producer is waiting ...");
                    Thread.Sleep(random.Next(1000));
                    while (produckt.Count == 0)
                    {
                        Console.WriteLine("putting stuff in queue");
                        for (int i = 0; i < 5; i++)
                        {
                            Console.WriteLine("putting " + i + " in queue");
                            produckt.Enqueue(i);
                            Thread.Sleep(random.Next(1000));
                        }
                    }
                }
                finally
                {
                    Monitor.Exit(baton);
                }
            }
        }
    }
}

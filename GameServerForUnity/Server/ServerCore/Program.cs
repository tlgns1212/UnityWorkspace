using System;

namespace ServerCore
{
    internal class Program
    {
        static int number = 0;
        static void T1()
        {
            for (int i = 0; i < 100000; i++)
                number++;
        }
        static void T2()
        {
            for (int i = 0; i < 100000; i++)
                number--;
        }
        static void Main(string[] args)
        {
            // 한것들 : Thread, ThreadPool, Task, TaskCreationOptions, Thread.MemoryBarrier(), Interlocked, Interlocked.Increment(ref num)
            // (Monitor.Enter(obj), Monitor.Exit(obj)) -> lock(obj)으로 대체, 

            Task t1 = new Task(T1);
            Task t2 = new Task(T2);

            t1.Start();
            t2.Start();
            Task.WaitAll(t1, t2);
            Console.WriteLine(number);


        }
    }
}
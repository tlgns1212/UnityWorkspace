using System;

namespace ServerCore
{
    internal class Program
    {
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(() => { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; });

        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if(repeat)
                Console.WriteLine(ThreadName.Value + "(repeat)");
            else
                Console.WriteLine(ThreadName.Value);

        }

        static void Main(string[] args)
        {
            // 한것들 : Thread, ThreadPool, Task, TaskCreationOptions, Thread.MemoryBarrier(), Interlocked, Interlocked.Increment(ref num)
            // (Monitor.Enter(obj), Monitor.Exit(obj)) -> lock(obj)으로 대체, AutoResetEvent (자동문같은 형식),
            // SpinLock, ReaderWriterLock, ThreadLocal<string>

            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);


        }
    }
}
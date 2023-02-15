using System;

namespace ServerCore
{
    class Lock
    {
        ManualResetEvent _available = new ManualResetEvent(true);

        public void Acquire()
        {
            _available.WaitOne();
            _available.Reset();
        }
        public void Release()
        {
            _available.Set();
        }
    }

    internal class Program
    {
        static int _num = 0;
        static Lock _lock = new Lock();

        static void T1()
        {
            for(int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num++;
                _lock.Release();
            }
        }
        static void T2()
        {
            for (int i = 0; i < 100000; i++)
            {
                _lock.Acquire();
                _num--;
                _lock.Release();
            }
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

            Console.WriteLine(_num);
        }
    }
}
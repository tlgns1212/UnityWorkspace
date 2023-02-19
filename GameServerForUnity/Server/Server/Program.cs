using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Collections.Specialized.BitVector32;
using ServerCore;

namespace Server
{
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected: {endPoint}");
            byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server!");
            Send(sendBuff);
            Thread.Sleep(1000);
            Disconnect();
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected: {endPoint}");
        }

        public override void OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Client] {recvData}");
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes: {numOfBytes}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            // 멀티쓰레드
            // 한것들 : Thread, ThreadPool, Task, TaskCreationOptions, Thread.MemoryBarrier(), Interlocked, Interlocked.Increment(ref num)
            // (Monitor.Enter(obj), Monitor.Exit(obj)) -> lock(obj)으로 대체, AutoResetEvent (자동문같은 형식),
            // SpinLock, ReaderWriterLock, ThreadLocal<string>


            // 네트워크 프로그래밍
            // DNS (Domain Name System), 비동기방식 : Socket.AsyncAccept(), 

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);



            _listener.Init(endPoint, () => { return new GameSession(); });
            Console.WriteLine("Listening...");

            while (true)
            {

            }

        }
    }
}
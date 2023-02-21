﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ServerCore;

namespace DummyClient
{
	public abstract class Packet
	{
		public ushort size;
		public ushort packetId;

		public abstract ArraySegment<byte> Write();
		public abstract void Read(ArraySegment<byte> s);
    }

	class PlayerInfoReq : Packet
	{
		public long playerId;

		public PlayerInfoReq()
		{
			this.packetId = (ushort)PacketID.PlayerInfoReq;
		}

        public override void Read(ArraySegment<byte> s)
        {
            ushort pos = 0;

            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            pos += 2;
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + pos);
            pos += 2;
            this.playerId = BitConverter.ToInt64(new ReadOnlySpan<byte>(s.Array, s.Offset + pos, s.Count - pos));

            pos += 8;
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> s = SendBufferHelper.Open(4096);

            ushort size = 0;
            bool success = true;

            size += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + size, s.Count - size), this.packetId);
            size += 2;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset + size, s.Count - size), this.playerId);
            size += 8;
            success &= BitConverter.TryWriteBytes(new Span<byte>(s.Array, s.Offset, s.Count), size);

			if (success == false)
				return null;

            return SendBufferHelper.Close(size);
        }
    }

	public enum PacketID
	{
		PlayerInfoReq = 1,
		PlayerInfoOk = 2,
	}

	class ServerSession : Session
	{
		static unsafe void ToBytes(byte[] array, int offset, ulong value)
		{
			fixed (byte* ptr = &array[offset])
				*(ulong*)ptr = value;
		}

		static unsafe void ToBytes<T>(byte[] array, int offset, T value) where T : unmanaged
		{
			fixed (byte* ptr = &array[offset])
				*(T*)ptr = value;
		}

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

			PlayerInfoReq packet = new PlayerInfoReq() {playerId = 1001 };


			// 보낸다
			for (int i = 0; i < 5; i++)
			{
				ArraySegment<byte> s = packet.Write();
				if (s != null)
					Send(s);
			}
		}

	

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override int OnRecv(ArraySegment<byte> buffer)
		{
			string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
			Console.WriteLine($"[From Server] {recvData}");
			return buffer.Count;
		}

		public override void OnSend(int numOfBytes)
		{
			Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}

}

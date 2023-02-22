using System;
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
		public string name;

		public struct SkillInfo
		{
			public int id;
			public short level;
			public float duration;
		}

		public List<SkillInfo> skills = new List<SkillInfo>();


		public PlayerInfoReq()
		{
			this.packetId = (ushort)PacketID.PlayerInfoReq;
		}

        public override void Read(ArraySegment<byte> segment)
        {
            ushort pos = 0;

			ReadOnlySpan<byte> s = new ReadOnlySpan<byte>(segment.Array, segment.Offset, segment.Count);

            //ushort size = BitConverter.ToUInt16(s.Array, s.Offset);
            pos += sizeof(ushort);
            //ushort id = BitConverter.ToUInt16(s.Array, s.Offset + pos);
            pos += sizeof(ushort);
            this.playerId = BitConverter.ToInt64(s.Slice(pos, s.Length - pos));
            pos += sizeof(long);

			// string
			ushort nameLen = BitConverter.ToUInt16(s.Slice(pos, s.Length - pos));
			pos += sizeof(ushort);
			this.name = Encoding.Unicode.GetString(s.Slice(pos, nameLen));
        }

        public override ArraySegment<byte> Write()
        {
            ArraySegment<byte> segment = SendBufferHelper.Open(4096);

            ushort size = 0;
            bool success = true;

			Span<byte> s = new Span<byte>(segment.Array, segment.Offset, segment.Count);


            size += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(size, s.Length - size), this.packetId);
            size += sizeof(ushort);
            success &= BitConverter.TryWriteBytes(s.Slice(size, s.Length - size), this.playerId);
            size += sizeof(long);            

			// string 
			ushort nameLen = (ushort)Encoding.Unicode.GetBytes(this.name, 0, this.name.Length, segment.Array, segment.Offset + size + sizeof(ushort));
            success &= BitConverter.TryWriteBytes(s.Slice(size, s.Length - size), nameLen);
            size += sizeof(ushort);
            size += nameLen;

            success &= BitConverter.TryWriteBytes(s, size);


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

			PlayerInfoReq packet = new PlayerInfoReq() {playerId = 1001 , name = "ABCD"};


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

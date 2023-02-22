using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
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

            //string 
            ushort nameLen = (ushort)Encoding.Unicode.GetByteCount(this.name);
            success &= BitConverter.TryWriteBytes(s.Slice(size, s.Length - size), nameLen);
            size += sizeof(ushort);
            Array.Copy(Encoding.Unicode.GetBytes(this.name), 0, segment.Array, size, nameLen);
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

    class ClientSession : PacketSession
	{
		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");
			Thread.Sleep(5000);
			Disconnect();
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			int pos = 0;

			ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset); 
			pos += 2;
			ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + pos);
			pos += 2;

			switch ((PacketID)id)
			{
				case PacketID.PlayerInfoReq:
					{
						PlayerInfoReq p = new PlayerInfoReq();
						p.Read(buffer);
                        Console.WriteLine($"PlayerInfoReq: {p.playerId}, {p.name}");
					}
					break;
				default:
					break;
			}

			Console.WriteLine($"RecvPacketId: {id}, Size {size}");
		}

		// TEMP
		public void Handle_PlayerInfoOk(ArraySegment<byte> buffer)
		{

		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}

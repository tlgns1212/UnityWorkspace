start ../../PacketGenerator/bin/Debug/net6.0/PacketGenerator.exe ../../PacketGenerator/PDL.xml
xcopy /Y GenPackets.cs "../../DummyClient/Packet"
xcopy /Y GenPackets.cs "../../Server/Packet"
xcopy /Y ClientPacketManager.cs "../../DummyClient/Packet"
xcopy /Y ServerPacketManager.cs "../../Server/Packet"
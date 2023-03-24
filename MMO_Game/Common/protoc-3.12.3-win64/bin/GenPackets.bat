protoc.exe -I=./ --csharp_out=./ ./Protocol.proto
IF ERRORLEVEL 1 PAUSE

start ../../../GameServerForUnity/PacketGenerator/bin/PacketGenerator.exe ./Protocol.proto
xcopy /Y Protocol.cs "../../../Client/Assets/Scripts/Packet"
xcopy /Y Protocol.cs "../../../GameServerForUnity/DummyClient/Packet"
xcopy /Y Protocol.cs "../../../GameServerForUnity/Server/Packet"
xcopy /Y ClientPacketManager.cs "../../../Client/Assets/Scripts/Packet"
xcopy /Y ClientPacketManager.cs "../../../GameServerForUnity/DummyClient/Packet"
xcopy /Y ServerPacketManager.cs "../../../GameServerForUnity/Server/Packet"
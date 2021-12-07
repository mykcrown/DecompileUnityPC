// Decompile from assembly: Assembly-CSharp.dll

using System;

namespace BattleServer
{
	public enum EBattleGameMsg
	{
		Msg_Relay,
		Msg_Input,
		Msg_HashCode,
		Msg_InputAck,
		Msg_RequestMissingInput,
		Msg_Ping,
		Msg_Pong,
		Msg_UdpPing,
		Msg_UdpPong,
		Msg_Timesync,
		Msg_TimesyncResponse,
		Msg_Disconnect,
		Msg_DisconnectAck,
		MsgCount
	}
}

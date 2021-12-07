using System;

namespace BattleServer
{
	// Token: 0x02000787 RID: 1927
	public enum EBattleGameMsg
	{
		// Token: 0x0400214F RID: 8527
		Msg_Relay,
		// Token: 0x04002150 RID: 8528
		Msg_Input,
		// Token: 0x04002151 RID: 8529
		Msg_HashCode,
		// Token: 0x04002152 RID: 8530
		Msg_InputAck,
		// Token: 0x04002153 RID: 8531
		Msg_RequestMissingInput,
		// Token: 0x04002154 RID: 8532
		Msg_Ping,
		// Token: 0x04002155 RID: 8533
		Msg_Pong,
		// Token: 0x04002156 RID: 8534
		Msg_UdpPing,
		// Token: 0x04002157 RID: 8535
		Msg_UdpPong,
		// Token: 0x04002158 RID: 8536
		Msg_Timesync,
		// Token: 0x04002159 RID: 8537
		Msg_TimesyncResponse,
		// Token: 0x0400215A RID: 8538
		Msg_Disconnect,
		// Token: 0x0400215B RID: 8539
		Msg_DisconnectAck,
		// Token: 0x0400215C RID: 8540
		MsgCount
	}
}

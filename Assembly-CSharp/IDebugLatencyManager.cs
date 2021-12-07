using System;
using IconsServer;
using network;

// Token: 0x020007D7 RID: 2007
public interface IDebugLatencyManager
{
	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x060031DC RID: 12764
	// (set) Token: 0x060031DD RID: 12765
	int AddInboundLatency { get; set; }

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x060031DE RID: 12766
	// (set) Token: 0x060031DF RID: 12767
	int AddOutboundLatency { get; set; }

	// Token: 0x060031E0 RID: 12768
	void ProcessBroadcast(ServerEvent message, Action<ServerEvent> callback);

	// Token: 0x060031E1 RID: 12769
	void ProcessSend(NetMsgBase message, Action<NetMsgBase> callback);
}

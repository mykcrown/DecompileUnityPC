using System;
using IconsServer;
using network;

// Token: 0x020007D6 RID: 2006
public class DebugLatencyManager : IDebugLatencyManager
{
	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x060031D2 RID: 12754 RVA: 0x000F228C File Offset: 0x000F068C
	// (set) Token: 0x060031D3 RID: 12755 RVA: 0x000F2294 File Offset: 0x000F0694
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x060031D4 RID: 12756 RVA: 0x000F229D File Offset: 0x000F069D
	// (set) Token: 0x060031D5 RID: 12757 RVA: 0x000F22A5 File Offset: 0x000F06A5
	[Inject]
	public IMainThreadTimer timer { get; set; }

	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x060031D6 RID: 12758 RVA: 0x000F22AE File Offset: 0x000F06AE
	// (set) Token: 0x060031D7 RID: 12759 RVA: 0x000F22B6 File Offset: 0x000F06B6
	public int AddOutboundLatency { get; set; }

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x060031D8 RID: 12760 RVA: 0x000F22BF File Offset: 0x000F06BF
	// (set) Token: 0x060031D9 RID: 12761 RVA: 0x000F22C7 File Offset: 0x000F06C7
	public int AddInboundLatency { get; set; }

	// Token: 0x060031DA RID: 12762 RVA: 0x000F22D0 File Offset: 0x000F06D0
	public void ProcessBroadcast(ServerEvent message, Action<ServerEvent> callback)
	{
		int addInboundLatency = this.AddInboundLatency;
		if (addInboundLatency <= 0)
		{
			callback(message);
		}
		else
		{
			if (message is BatchEvent)
			{
				message = (ServerEvent)(message as BatchEvent).Clone();
			}
			this.timer.SetTimeout(addInboundLatency, delegate
			{
				callback(message);
			});
		}
	}

	// Token: 0x060031DB RID: 12763 RVA: 0x000F2358 File Offset: 0x000F0758
	public void ProcessSend(NetMsgBase message, Action<NetMsgBase> callback)
	{
		int addOutboundLatency = this.AddOutboundLatency;
		if (addOutboundLatency <= 0)
		{
			callback(message);
		}
		else
		{
			this.timer.SetTimeout(addOutboundLatency, delegate
			{
				callback(message);
			});
		}
	}
}

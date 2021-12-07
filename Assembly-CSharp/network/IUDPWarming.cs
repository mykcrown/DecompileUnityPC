using System;
using Steamworks;

namespace network
{
	// Token: 0x0200083C RID: 2108
	public interface IUDPWarming
	{
		// Token: 0x060034B7 RID: 13495
		void Reset();

		// Token: 0x060034B8 RID: 13496
		void Tick();

		// Token: 0x060034B9 RID: 13497
		void BeginUdpWarm();

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060034BA RID: 13498
		bool IsAllConnectionsReady { get; }

		// Token: 0x060034BB RID: 13499
		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);
	}
}

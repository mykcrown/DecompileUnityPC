using System;
using Steamworks;

namespace network
{
	// Token: 0x02000838 RID: 2104
	public interface IPingManager
	{
		// Token: 0x06003487 RID: 13447
		void Ping();

		// Token: 0x06003488 RID: 13448
		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);

		// Token: 0x06003489 RID: 13449
		void Tick();

		// Token: 0x17000CB8 RID: 3256
		// (get) Token: 0x0600348A RID: 13450
		long PingTimeMs { get; }

		// Token: 0x17000CB9 RID: 3257
		// (get) Token: 0x0600348B RID: 13451
		long LatencyMs { get; }
	}
}

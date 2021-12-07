using System;
using System.Collections.Generic;
using Steamworks;

namespace network
{
	// Token: 0x0200083A RID: 2106
	public interface ITimeSynchronizer
	{
		// Token: 0x060034A1 RID: 13473
		long GetTimeOffsetMs();

		// Token: 0x060034A2 RID: 13474
		void Reset();

		// Token: 0x060034A3 RID: 13475
		void KeepOnlyThesePlayers(List<SteamManager.SteamLobbyData> list);

		// Token: 0x060034A4 RID: 13476
		void Tick();

		// Token: 0x060034A5 RID: 13477
		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);

		// Token: 0x17000CBE RID: 3262
		// (get) Token: 0x060034A6 RID: 13478
		bool IsSynchronizationComplete { get; }

		// Token: 0x060034A7 RID: 13479
		void SetUpdateCallback(Action callback);
	}
}

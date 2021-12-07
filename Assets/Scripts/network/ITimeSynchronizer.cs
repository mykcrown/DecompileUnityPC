// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;
using System.Collections.Generic;

namespace network
{
	public interface ITimeSynchronizer
	{
		bool IsSynchronizationComplete
		{
			get;
		}

		long GetTimeOffsetMs();

		void Reset();

		void KeepOnlyThesePlayers(List<SteamManager.SteamLobbyData> list);

		void Tick();

		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);

		void SetUpdateCallback(Action callback);
	}
}

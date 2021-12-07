// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;

namespace network
{
	public interface IPingManager
	{
		long PingTimeMs
		{
			get;
		}

		long LatencyMs
		{
			get;
		}

		void Ping();

		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);

		void Tick();
	}
}

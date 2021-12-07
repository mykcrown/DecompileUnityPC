// Decompile from assembly: Assembly-CSharp.dll

using Steamworks;
using System;

namespace network
{
	public interface IUDPWarming
	{
		bool IsAllConnectionsReady
		{
			get;
		}

		void Reset();

		void Tick();

		void BeginUdpWarm();

		bool ScanIncomingMessage(byte msgId, byte[] buffer, uint bufferSize, CSteamID senderSteamID);
	}
}

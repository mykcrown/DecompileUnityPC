// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(5201)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SearchForGameProgressCallback_t
	{
		public const int k_iCallback = 5201;

		public ulong m_ullSearchID;

		public EResult m_eResult;

		public CSteamID m_lobbyID;

		public CSteamID m_steamIDEndedSearch;

		public int m_nSecondsRemainingEstimate;

		public int m_cPlayersSearching;
	}
}

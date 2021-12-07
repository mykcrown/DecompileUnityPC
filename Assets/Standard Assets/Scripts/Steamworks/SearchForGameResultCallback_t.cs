// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(5202)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SearchForGameResultCallback_t
	{
		public const int k_iCallback = 5202;

		public ulong m_ullSearchID;

		public EResult m_eResult;

		public int m_nCountPlayersInGame;

		public int m_nCountAcceptedGame;

		public CSteamID m_steamIDHost;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bFinalCallback;
	}
}

// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(1101)]
	[StructLayout(LayoutKind.Explicit, Pack = 8)]
	public struct UserStatsReceived_t
	{
		public const int k_iCallback = 1101;

		[FieldOffset(0)]
		public ulong m_nGameID;

		[FieldOffset(8)]
		public EResult m_eResult;

		[FieldOffset(12)]
		public CSteamID m_steamIDUser;
	}
}

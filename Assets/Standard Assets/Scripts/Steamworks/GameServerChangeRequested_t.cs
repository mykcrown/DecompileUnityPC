// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(332)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GameServerChangeRequested_t
	{
		public const int k_iCallback = 332;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchServer;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string m_rgchPassword;
	}
}

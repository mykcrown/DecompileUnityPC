// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(4624)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct GetOPFSettingsResult_t
	{
		public const int k_iCallback = 4624;

		public EResult m_eResult;

		public AppId_t m_unVideoAppID;
	}
}

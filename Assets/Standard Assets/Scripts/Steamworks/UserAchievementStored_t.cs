// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(1103)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct UserAchievementStored_t
	{
		public const int k_iCallback = 1103;

		public ulong m_nGameID;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bGroupAchievement;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string m_rgchAchievementName;

		public uint m_nCurProgress;

		public uint m_nMaxProgress;
	}
}

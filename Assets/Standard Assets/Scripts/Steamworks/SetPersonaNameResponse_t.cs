// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(347)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetPersonaNameResponse_t
	{
		public const int k_iCallback = 347;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bSuccess;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bLocalSuccess;

		public EResult m_result;
	}
}

// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(3408)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct SetUserItemVoteResult_t
	{
		public const int k_iCallback = 3408;

		public PublishedFileId_t m_nPublishedFileId;

		public EResult m_eResult;

		[MarshalAs(UnmanagedType.I1)]
		public bool m_bVoteUp;
	}
}

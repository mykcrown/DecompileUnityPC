// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(1320)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct RemoteStorageGetPublishedItemVoteDetailsResult_t
	{
		public const int k_iCallback = 1320;

		public EResult m_eResult;

		public PublishedFileId_t m_unPublishedFileId;

		public int m_nVotesFor;

		public int m_nVotesAgainst;

		public int m_nReports;

		public float m_fScore;
	}
}

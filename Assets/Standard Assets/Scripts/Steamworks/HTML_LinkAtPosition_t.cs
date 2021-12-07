// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	[CallbackIdentity(4513)]
	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct HTML_LinkAtPosition_t
	{
		public const int k_iCallback = 4513;

		public HHTMLBrowser unBrowserHandle;

		public uint x;

		public uint y;

		public string pchURL;

		[MarshalAs(UnmanagedType.I1)]
		public bool bInput;

		[MarshalAs(UnmanagedType.I1)]
		public bool bLiveLink;
	}
}

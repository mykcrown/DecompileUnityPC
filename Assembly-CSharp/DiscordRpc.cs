using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x020001F2 RID: 498
public class DiscordRpc
{
	// Token: 0x06000943 RID: 2371
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister, string optionalSteamId);

	// Token: 0x06000944 RID: 2372
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	// Token: 0x06000945 RID: 2373
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
	public static extern void RunCallbacks();

	// Token: 0x06000946 RID: 2374
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
	private static extern void UpdatePresenceNative(ref DiscordRpc.RichPresenceStruct presence);

	// Token: 0x06000947 RID: 2375
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_ClearPresence")]
	public static extern void ClearPresence();

	// Token: 0x06000948 RID: 2376
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Respond")]
	public static extern void Respond(string userId, DiscordRpc.Reply reply);

	// Token: 0x06000949 RID: 2377
	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdateHandlers")]
	public static extern void UpdateHandlers(ref DiscordRpc.EventHandlers handlers);

	// Token: 0x0600094A RID: 2378 RVA: 0x0004F694 File Offset: 0x0004DA94
	public static void UpdatePresence(DiscordRpc.RichPresence presence)
	{
		DiscordRpc.RichPresenceStruct @struct = presence.GetStruct();
		DiscordRpc.UpdatePresenceNative(ref @struct);
		presence.FreeMem();
	}

	// Token: 0x020001F3 RID: 499
	// (Invoke) Token: 0x0600094C RID: 2380
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser);

	// Token: 0x020001F4 RID: 500
	// (Invoke) Token: 0x06000950 RID: 2384
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisconnectedCallback(int errorCode, string message);

	// Token: 0x020001F5 RID: 501
	// (Invoke) Token: 0x06000954 RID: 2388
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(int errorCode, string message);

	// Token: 0x020001F6 RID: 502
	// (Invoke) Token: 0x06000958 RID: 2392
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JoinCallback(string secret);

	// Token: 0x020001F7 RID: 503
	// (Invoke) Token: 0x0600095C RID: 2396
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SpectateCallback(string secret);

	// Token: 0x020001F8 RID: 504
	// (Invoke) Token: 0x06000960 RID: 2400
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void RequestCallback(ref DiscordRpc.DiscordUser request);

	// Token: 0x020001F9 RID: 505
	public struct EventHandlers
	{
		// Token: 0x0400067A RID: 1658
		public DiscordRpc.ReadyCallback readyCallback;

		// Token: 0x0400067B RID: 1659
		public DiscordRpc.DisconnectedCallback disconnectedCallback;

		// Token: 0x0400067C RID: 1660
		public DiscordRpc.ErrorCallback errorCallback;

		// Token: 0x0400067D RID: 1661
		public DiscordRpc.JoinCallback joinCallback;

		// Token: 0x0400067E RID: 1662
		public DiscordRpc.SpectateCallback spectateCallback;

		// Token: 0x0400067F RID: 1663
		public DiscordRpc.RequestCallback requestCallback;
	}

	// Token: 0x020001FA RID: 506
	[Serializable]
	public struct RichPresenceStruct
	{
		// Token: 0x04000680 RID: 1664
		public IntPtr state;

		// Token: 0x04000681 RID: 1665
		public IntPtr details;

		// Token: 0x04000682 RID: 1666
		public long startTimestamp;

		// Token: 0x04000683 RID: 1667
		public long endTimestamp;

		// Token: 0x04000684 RID: 1668
		public IntPtr largeImageKey;

		// Token: 0x04000685 RID: 1669
		public IntPtr largeImageText;

		// Token: 0x04000686 RID: 1670
		public IntPtr smallImageKey;

		// Token: 0x04000687 RID: 1671
		public IntPtr smallImageText;

		// Token: 0x04000688 RID: 1672
		public IntPtr partyId;

		// Token: 0x04000689 RID: 1673
		public int partySize;

		// Token: 0x0400068A RID: 1674
		public int partyMax;

		// Token: 0x0400068B RID: 1675
		public IntPtr matchSecret;

		// Token: 0x0400068C RID: 1676
		public IntPtr joinSecret;

		// Token: 0x0400068D RID: 1677
		public IntPtr spectateSecret;

		// Token: 0x0400068E RID: 1678
		public bool instance;
	}

	// Token: 0x020001FB RID: 507
	[Serializable]
	public struct DiscordUser
	{
		// Token: 0x0400068F RID: 1679
		public string userId;

		// Token: 0x04000690 RID: 1680
		public string username;

		// Token: 0x04000691 RID: 1681
		public string discriminator;

		// Token: 0x04000692 RID: 1682
		public string avatar;
	}

	// Token: 0x020001FC RID: 508
	public enum Reply
	{
		// Token: 0x04000694 RID: 1684
		No,
		// Token: 0x04000695 RID: 1685
		Yes,
		// Token: 0x04000696 RID: 1686
		Ignore
	}

	// Token: 0x020001FD RID: 509
	public class RichPresence
	{
		// Token: 0x06000964 RID: 2404 RVA: 0x0004F6CC File Offset: 0x0004DACC
		internal DiscordRpc.RichPresenceStruct GetStruct()
		{
			if (this._buffers.Count > 0)
			{
				this.FreeMem();
			}
			this._presence.state = this.StrToPtr(this.state);
			this._presence.details = this.StrToPtr(this.details);
			this._presence.startTimestamp = this.startTimestamp;
			this._presence.endTimestamp = this.endTimestamp;
			this._presence.largeImageKey = this.StrToPtr(this.largeImageKey);
			this._presence.largeImageText = this.StrToPtr(this.largeImageText);
			this._presence.smallImageKey = this.StrToPtr(this.smallImageKey);
			this._presence.smallImageText = this.StrToPtr(this.smallImageText);
			this._presence.partyId = this.StrToPtr(this.partyId);
			this._presence.partySize = this.partySize;
			this._presence.partyMax = this.partyMax;
			this._presence.matchSecret = this.StrToPtr(this.matchSecret);
			this._presence.joinSecret = this.StrToPtr(this.joinSecret);
			this._presence.spectateSecret = this.StrToPtr(this.spectateSecret);
			this._presence.instance = this.instance;
			return this._presence;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0004F834 File Offset: 0x0004DC34
		private IntPtr StrToPtr(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				return IntPtr.Zero;
			}
			int byteCount = Encoding.UTF8.GetByteCount(input);
			IntPtr intPtr = Marshal.AllocHGlobal(byteCount + 1);
			for (int i = 0; i < byteCount + 1; i++)
			{
				Marshal.WriteByte(intPtr, i, 0);
			}
			this._buffers.Add(intPtr);
			Marshal.Copy(Encoding.UTF8.GetBytes(input), 0, intPtr, byteCount);
			return intPtr;
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0004F8A4 File Offset: 0x0004DCA4
		private static string StrToUtf8NullTerm(string toconv)
		{
			string text = toconv.Trim();
			byte[] bytes = Encoding.Default.GetBytes(text);
			if (bytes.Length > 0 && bytes[bytes.Length - 1] != 0)
			{
				text += "\0\0";
			}
			return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(text));
		}

		// Token: 0x06000967 RID: 2407 RVA: 0x0004F8FC File Offset: 0x0004DCFC
		internal void FreeMem()
		{
			for (int i = this._buffers.Count - 1; i >= 0; i--)
			{
				Marshal.FreeHGlobal(this._buffers[i]);
				this._buffers.RemoveAt(i);
			}
		}

		// Token: 0x04000697 RID: 1687
		private DiscordRpc.RichPresenceStruct _presence;

		// Token: 0x04000698 RID: 1688
		private readonly List<IntPtr> _buffers = new List<IntPtr>(10);

		// Token: 0x04000699 RID: 1689
		public string state;

		// Token: 0x0400069A RID: 1690
		public string details;

		// Token: 0x0400069B RID: 1691
		public long startTimestamp;

		// Token: 0x0400069C RID: 1692
		public long endTimestamp;

		// Token: 0x0400069D RID: 1693
		public string largeImageKey;

		// Token: 0x0400069E RID: 1694
		public string largeImageText;

		// Token: 0x0400069F RID: 1695
		public string smallImageKey;

		// Token: 0x040006A0 RID: 1696
		public string smallImageText;

		// Token: 0x040006A1 RID: 1697
		public string partyId;

		// Token: 0x040006A2 RID: 1698
		public int partySize;

		// Token: 0x040006A3 RID: 1699
		public int partyMax;

		// Token: 0x040006A4 RID: 1700
		public string matchSecret;

		// Token: 0x040006A5 RID: 1701
		public string joinSecret;

		// Token: 0x040006A6 RID: 1702
		public string spectateSecret;

		// Token: 0x040006A7 RID: 1703
		public bool instance;
	}
}

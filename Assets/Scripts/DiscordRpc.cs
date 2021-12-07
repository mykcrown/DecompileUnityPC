// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

public class DiscordRpc
{
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ReadyCallback(ref DiscordRpc.DiscordUser connectedUser);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void DisconnectedCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void ErrorCallback(int errorCode, string message);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void JoinCallback(string secret);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void SpectateCallback(string secret);

	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void RequestCallback(ref DiscordRpc.DiscordUser request);

	public struct EventHandlers
	{
		public DiscordRpc.ReadyCallback readyCallback;

		public DiscordRpc.DisconnectedCallback disconnectedCallback;

		public DiscordRpc.ErrorCallback errorCallback;

		public DiscordRpc.JoinCallback joinCallback;

		public DiscordRpc.SpectateCallback spectateCallback;

		public DiscordRpc.RequestCallback requestCallback;
	}

	[Serializable]
	public struct RichPresenceStruct
	{
		public IntPtr state;

		public IntPtr details;

		public long startTimestamp;

		public long endTimestamp;

		public IntPtr largeImageKey;

		public IntPtr largeImageText;

		public IntPtr smallImageKey;

		public IntPtr smallImageText;

		public IntPtr partyId;

		public int partySize;

		public int partyMax;

		public IntPtr matchSecret;

		public IntPtr joinSecret;

		public IntPtr spectateSecret;

		public bool instance;
	}

	[Serializable]
	public struct DiscordUser
	{
		public string userId;

		public string username;

		public string discriminator;

		public string avatar;
	}

	public enum Reply
	{
		No,
		Yes,
		Ignore
	}

	public class RichPresence
	{
		private DiscordRpc.RichPresenceStruct _presence;

		private readonly List<IntPtr> _buffers = new List<IntPtr>(10);

		public string state;

		public string details;

		public long startTimestamp;

		public long endTimestamp;

		public string largeImageKey;

		public string largeImageText;

		public string smallImageKey;

		public string smallImageText;

		public string partyId;

		public int partySize;

		public int partyMax;

		public string matchSecret;

		public string joinSecret;

		public string spectateSecret;

		public bool instance;

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

		internal void FreeMem()
		{
			for (int i = this._buffers.Count - 1; i >= 0; i--)
			{
				Marshal.FreeHGlobal(this._buffers[i]);
				this._buffers.RemoveAt(i);
			}
		}
	}

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Initialize")]
	public static extern void Initialize(string applicationId, ref DiscordRpc.EventHandlers handlers, bool autoRegister, string optionalSteamId);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Shutdown")]
	public static extern void Shutdown();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_RunCallbacks")]
	public static extern void RunCallbacks();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdatePresence")]
	private static extern void UpdatePresenceNative(ref DiscordRpc.RichPresenceStruct presence);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_ClearPresence")]
	public static extern void ClearPresence();

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_Respond")]
	public static extern void Respond(string userId, DiscordRpc.Reply reply);

	[DllImport("discord-rpc", CallingConvention = CallingConvention.Cdecl, EntryPoint = "Discord_UpdateHandlers")]
	public static extern void UpdateHandlers(ref DiscordRpc.EventHandlers handlers);

	public static void UpdatePresence(DiscordRpc.RichPresence presence)
	{
		DiscordRpc.RichPresenceStruct @struct = presence.GetStruct();
		DiscordRpc.UpdatePresenceNative(ref @struct);
		presence.FreeMem();
	}
}

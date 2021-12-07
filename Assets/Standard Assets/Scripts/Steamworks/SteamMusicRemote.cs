// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
	public static class SteamMusicRemote
	{
		public static bool RegisterSteamMusicRemote(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamMusicRemote_RegisterSteamMusicRemote(CSteamAPIContext.GetSteamMusicRemote(), uTF8StringHandle);
			}
			return result;
		}

		public static bool DeregisterSteamMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_DeregisterSteamMusicRemote(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool BIsCurrentMusicRemote()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BIsCurrentMusicRemote(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool BActivationSuccess(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_BActivationSuccess(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool SetDisplayName(string pchDisplayName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchDisplayName))
			{
				result = NativeMethods.ISteamMusicRemote_SetDisplayName(CSteamAPIContext.GetSteamMusicRemote(), uTF8StringHandle);
			}
			return result;
		}

		public static bool SetPNGIcon_64x64(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetPNGIcon_64x64(CSteamAPIContext.GetSteamMusicRemote(), pvBuffer, cbBufferLength);
		}

		public static bool EnablePlayPrevious(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayPrevious(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool EnablePlayNext(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlayNext(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool EnableShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableShuffled(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool EnableLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableLooped(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool EnableQueue(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnableQueue(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool EnablePlaylists(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_EnablePlaylists(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool UpdatePlaybackStatus(AudioPlayback_Status nStatus)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdatePlaybackStatus(CSteamAPIContext.GetSteamMusicRemote(), nStatus);
		}

		public static bool UpdateShuffled(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateShuffled(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool UpdateLooped(bool bValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateLooped(CSteamAPIContext.GetSteamMusicRemote(), bValue);
		}

		public static bool UpdateVolume(float flValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateVolume(CSteamAPIContext.GetSteamMusicRemote(), flValue);
		}

		public static bool CurrentEntryWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool CurrentEntryIsAvailable(bool bAvailable)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryIsAvailable(CSteamAPIContext.GetSteamMusicRemote(), bAvailable);
		}

		public static bool UpdateCurrentEntryText(string pchText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchText))
			{
				result = NativeMethods.ISteamMusicRemote_UpdateCurrentEntryText(CSteamAPIContext.GetSteamMusicRemote(), uTF8StringHandle);
			}
			return result;
		}

		public static bool UpdateCurrentEntryElapsedSeconds(int nValue)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryElapsedSeconds(CSteamAPIContext.GetSteamMusicRemote(), nValue);
		}

		public static bool UpdateCurrentEntryCoverArt(byte[] pvBuffer, uint cbBufferLength)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_UpdateCurrentEntryCoverArt(CSteamAPIContext.GetSteamMusicRemote(), pvBuffer, cbBufferLength);
		}

		public static bool CurrentEntryDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_CurrentEntryDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool QueueWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool ResetQueueEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetQueueEntries(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool SetQueueEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetQueueEntry(CSteamAPIContext.GetSteamMusicRemote(), nID, nPosition, uTF8StringHandle);
			}
			return result;
		}

		public static bool SetCurrentQueueEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentQueueEntry(CSteamAPIContext.GetSteamMusicRemote(), nID);
		}

		public static bool QueueDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_QueueDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool PlaylistWillChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistWillChange(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool ResetPlaylistEntries()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_ResetPlaylistEntries(CSteamAPIContext.GetSteamMusicRemote());
		}

		public static bool SetPlaylistEntry(int nID, int nPosition, string pchEntryText)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchEntryText))
			{
				result = NativeMethods.ISteamMusicRemote_SetPlaylistEntry(CSteamAPIContext.GetSteamMusicRemote(), nID, nPosition, uTF8StringHandle);
			}
			return result;
		}

		public static bool SetCurrentPlaylistEntry(int nID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_SetCurrentPlaylistEntry(CSteamAPIContext.GetSteamMusicRemote(), nID);
		}

		public static bool PlaylistDidChange()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamMusicRemote_PlaylistDidChange(CSteamAPIContext.GetSteamMusicRemote());
		}
	}
}

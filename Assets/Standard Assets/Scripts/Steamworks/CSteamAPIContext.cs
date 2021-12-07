// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
	internal static class CSteamAPIContext
	{
		private static IntPtr m_pSteamClient;

		private static IntPtr m_pSteamUser;

		private static IntPtr m_pSteamFriends;

		private static IntPtr m_pSteamUtils;

		private static IntPtr m_pSteamMatchmaking;

		private static IntPtr m_pSteamUserStats;

		private static IntPtr m_pSteamApps;

		private static IntPtr m_pSteamMatchmakingServers;

		private static IntPtr m_pSteamNetworking;

		private static IntPtr m_pSteamRemoteStorage;

		private static IntPtr m_pSteamScreenshots;

		private static IntPtr m_pSteamGameSearch;

		private static IntPtr m_pSteamHTTP;

		private static IntPtr m_pController;

		private static IntPtr m_pSteamUGC;

		private static IntPtr m_pSteamAppList;

		private static IntPtr m_pSteamMusic;

		private static IntPtr m_pSteamMusicRemote;

		private static IntPtr m_pSteamHTMLSurface;

		private static IntPtr m_pSteamInventory;

		private static IntPtr m_pSteamVideo;

		private static IntPtr m_pSteamParentalSettings;

		private static IntPtr m_pSteamInput;

		private static IntPtr m_pSteamParties;

		internal static void Clear()
		{
			CSteamAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUser = IntPtr.Zero;
			CSteamAPIContext.m_pSteamFriends = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmaking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUserStats = IntPtr.Zero;
			CSteamAPIContext.m_pSteamApps = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMatchmakingServers = IntPtr.Zero;
			CSteamAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamAPIContext.m_pSteamRemoteStorage = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamAPIContext.m_pSteamScreenshots = IntPtr.Zero;
			CSteamAPIContext.m_pSteamGameSearch = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pController = IntPtr.Zero;
			CSteamAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamAPIContext.m_pSteamAppList = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusic = IntPtr.Zero;
			CSteamAPIContext.m_pSteamMusicRemote = IntPtr.Zero;
			CSteamAPIContext.m_pSteamHTMLSurface = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamAPIContext.m_pSteamVideo = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParentalSettings = IntPtr.Zero;
			CSteamAPIContext.m_pSteamInput = IntPtr.Zero;
			CSteamAPIContext.m_pSteamParties = IntPtr.Zero;
		}

		internal static bool Init()
		{
			HSteamUser hSteamUser = SteamAPI.GetHSteamUser();
			HSteamPipe hSteamPipe = SteamAPI.GetHSteamPipe();
			if (hSteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient018"))
			{
				CSteamAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(uTF8StringHandle);
			}
			if (CSteamAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUser = SteamClient.GetISteamUser(hSteamUser, hSteamPipe, "SteamUser020");
			if (CSteamAPIContext.m_pSteamUser == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamFriends = SteamClient.GetISteamFriends(hSteamUser, hSteamPipe, "SteamFriends017");
			if (CSteamAPIContext.m_pSteamFriends == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUtils = SteamClient.GetISteamUtils(hSteamPipe, "SteamUtils009");
			if (CSteamAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmaking = SteamClient.GetISteamMatchmaking(hSteamUser, hSteamPipe, "SteamMatchMaking009");
			if (CSteamAPIContext.m_pSteamMatchmaking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMatchmakingServers = SteamClient.GetISteamMatchmakingServers(hSteamUser, hSteamPipe, "SteamMatchMakingServers002");
			if (CSteamAPIContext.m_pSteamMatchmakingServers == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUserStats = SteamClient.GetISteamUserStats(hSteamUser, hSteamPipe, "STEAMUSERSTATS_INTERFACE_VERSION011");
			if (CSteamAPIContext.m_pSteamUserStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamApps = SteamClient.GetISteamApps(hSteamUser, hSteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
			if (CSteamAPIContext.m_pSteamApps == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamNetworking = SteamClient.GetISteamNetworking(hSteamUser, hSteamPipe, "SteamNetworking005");
			if (CSteamAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamRemoteStorage = SteamClient.GetISteamRemoteStorage(hSteamUser, hSteamPipe, "STEAMREMOTESTORAGE_INTERFACE_VERSION014");
			if (CSteamAPIContext.m_pSteamRemoteStorage == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamScreenshots = SteamClient.GetISteamScreenshots(hSteamUser, hSteamPipe, "STEAMSCREENSHOTS_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamScreenshots == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamGameSearch = SteamClient.GetISteamGameSearch(hSteamUser, hSteamPipe, "SteamMatchGameSearch001");
			if (CSteamAPIContext.m_pSteamGameSearch == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTTP = SteamClient.GetISteamHTTP(hSteamUser, hSteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pController = SteamClient.GetISteamController(hSteamUser, hSteamPipe, "SteamController007");
			if (CSteamAPIContext.m_pController == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamUGC = SteamClient.GetISteamUGC(hSteamUser, hSteamPipe, "STEAMUGC_INTERFACE_VERSION012");
			if (CSteamAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamAppList = SteamClient.GetISteamAppList(hSteamUser, hSteamPipe, "STEAMAPPLIST_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamAppList == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMusic = SteamClient.GetISteamMusic(hSteamUser, hSteamPipe, "STEAMMUSIC_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamMusic == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamMusicRemote = SteamClient.GetISteamMusicRemote(hSteamUser, hSteamPipe, "STEAMMUSICREMOTE_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamMusicRemote == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamHTMLSurface = SteamClient.GetISteamHTMLSurface(hSteamUser, hSteamPipe, "STEAMHTMLSURFACE_INTERFACE_VERSION_005");
			if (CSteamAPIContext.m_pSteamHTMLSurface == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInventory = SteamClient.GetISteamInventory(hSteamUser, hSteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamVideo = SteamClient.GetISteamVideo(hSteamUser, hSteamPipe, "STEAMVIDEO_INTERFACE_V002");
			if (CSteamAPIContext.m_pSteamVideo == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParentalSettings = SteamClient.GetISteamParentalSettings(hSteamUser, hSteamPipe, "STEAMPARENTALSETTINGS_INTERFACE_VERSION001");
			if (CSteamAPIContext.m_pSteamParentalSettings == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamInput = SteamClient.GetISteamInput(hSteamUser, hSteamPipe, "SteamInput001");
			if (CSteamAPIContext.m_pSteamInput == IntPtr.Zero)
			{
				return false;
			}
			CSteamAPIContext.m_pSteamParties = SteamClient.GetISteamParties(hSteamUser, hSteamPipe, "SteamParties002");
			return !(CSteamAPIContext.m_pSteamParties == IntPtr.Zero);
		}

		internal static IntPtr GetSteamClient()
		{
			return CSteamAPIContext.m_pSteamClient;
		}

		internal static IntPtr GetSteamUser()
		{
			return CSteamAPIContext.m_pSteamUser;
		}

		internal static IntPtr GetSteamFriends()
		{
			return CSteamAPIContext.m_pSteamFriends;
		}

		internal static IntPtr GetSteamUtils()
		{
			return CSteamAPIContext.m_pSteamUtils;
		}

		internal static IntPtr GetSteamMatchmaking()
		{
			return CSteamAPIContext.m_pSteamMatchmaking;
		}

		internal static IntPtr GetSteamUserStats()
		{
			return CSteamAPIContext.m_pSteamUserStats;
		}

		internal static IntPtr GetSteamApps()
		{
			return CSteamAPIContext.m_pSteamApps;
		}

		internal static IntPtr GetSteamMatchmakingServers()
		{
			return CSteamAPIContext.m_pSteamMatchmakingServers;
		}

		internal static IntPtr GetSteamNetworking()
		{
			return CSteamAPIContext.m_pSteamNetworking;
		}

		internal static IntPtr GetSteamRemoteStorage()
		{
			return CSteamAPIContext.m_pSteamRemoteStorage;
		}

		internal static IntPtr GetSteamScreenshots()
		{
			return CSteamAPIContext.m_pSteamScreenshots;
		}

		internal static IntPtr GetSteamGameSearch()
		{
			return CSteamAPIContext.m_pSteamGameSearch;
		}

		internal static IntPtr GetSteamHTTP()
		{
			return CSteamAPIContext.m_pSteamHTTP;
		}

		internal static IntPtr GetSteamController()
		{
			return CSteamAPIContext.m_pController;
		}

		internal static IntPtr GetSteamUGC()
		{
			return CSteamAPIContext.m_pSteamUGC;
		}

		internal static IntPtr GetSteamAppList()
		{
			return CSteamAPIContext.m_pSteamAppList;
		}

		internal static IntPtr GetSteamMusic()
		{
			return CSteamAPIContext.m_pSteamMusic;
		}

		internal static IntPtr GetSteamMusicRemote()
		{
			return CSteamAPIContext.m_pSteamMusicRemote;
		}

		internal static IntPtr GetSteamHTMLSurface()
		{
			return CSteamAPIContext.m_pSteamHTMLSurface;
		}

		internal static IntPtr GetSteamInventory()
		{
			return CSteamAPIContext.m_pSteamInventory;
		}

		internal static IntPtr GetSteamVideo()
		{
			return CSteamAPIContext.m_pSteamVideo;
		}

		internal static IntPtr GetSteamParentalSettings()
		{
			return CSteamAPIContext.m_pSteamParentalSettings;
		}

		internal static IntPtr GetSteamInput()
		{
			return CSteamAPIContext.m_pSteamInput;
		}

		internal static IntPtr GetSteamParties()
		{
			return CSteamAPIContext.m_pSteamParties;
		}
	}
}

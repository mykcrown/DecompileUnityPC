// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;

namespace Steamworks
{
	internal static class CSteamGameServerAPIContext
	{
		private static IntPtr m_pSteamClient;

		private static IntPtr m_pSteamGameServer;

		private static IntPtr m_pSteamUtils;

		private static IntPtr m_pSteamNetworking;

		private static IntPtr m_pSteamGameServerStats;

		private static IntPtr m_pSteamHTTP;

		private static IntPtr m_pSteamInventory;

		private static IntPtr m_pSteamUGC;

		private static IntPtr m_pSteamApps;

		internal static void Clear()
		{
			CSteamGameServerAPIContext.m_pSteamClient = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServer = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUtils = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamNetworking = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamGameServerStats = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamHTTP = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamInventory = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamUGC = IntPtr.Zero;
			CSteamGameServerAPIContext.m_pSteamApps = IntPtr.Zero;
		}

		internal static bool Init()
		{
			HSteamUser hSteamUser = GameServer.GetHSteamUser();
			HSteamPipe hSteamPipe = GameServer.GetHSteamPipe();
			if (hSteamPipe == (HSteamPipe)0)
			{
				return false;
			}
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle("SteamClient018"))
			{
				CSteamGameServerAPIContext.m_pSteamClient = NativeMethods.SteamInternal_CreateInterface(uTF8StringHandle);
			}
			if (CSteamGameServerAPIContext.m_pSteamClient == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServer = SteamGameServerClient.GetISteamGameServer(hSteamUser, hSteamPipe, "SteamGameServer012");
			if (CSteamGameServerAPIContext.m_pSteamGameServer == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUtils = SteamGameServerClient.GetISteamUtils(hSteamPipe, "SteamUtils009");
			if (CSteamGameServerAPIContext.m_pSteamUtils == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamNetworking = SteamGameServerClient.GetISteamNetworking(hSteamUser, hSteamPipe, "SteamNetworking005");
			if (CSteamGameServerAPIContext.m_pSteamNetworking == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamGameServerStats = SteamGameServerClient.GetISteamGameServerStats(hSteamUser, hSteamPipe, "SteamGameServerStats001");
			if (CSteamGameServerAPIContext.m_pSteamGameServerStats == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamHTTP = SteamGameServerClient.GetISteamHTTP(hSteamUser, hSteamPipe, "STEAMHTTP_INTERFACE_VERSION003");
			if (CSteamGameServerAPIContext.m_pSteamHTTP == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamInventory = SteamGameServerClient.GetISteamInventory(hSteamUser, hSteamPipe, "STEAMINVENTORY_INTERFACE_V003");
			if (CSteamGameServerAPIContext.m_pSteamInventory == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamUGC = SteamGameServerClient.GetISteamUGC(hSteamUser, hSteamPipe, "STEAMUGC_INTERFACE_VERSION012");
			if (CSteamGameServerAPIContext.m_pSteamUGC == IntPtr.Zero)
			{
				return false;
			}
			CSteamGameServerAPIContext.m_pSteamApps = SteamGameServerClient.GetISteamApps(hSteamUser, hSteamPipe, "STEAMAPPS_INTERFACE_VERSION008");
			return !(CSteamGameServerAPIContext.m_pSteamApps == IntPtr.Zero);
		}

		internal static IntPtr GetSteamClient()
		{
			return CSteamGameServerAPIContext.m_pSteamClient;
		}

		internal static IntPtr GetSteamGameServer()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServer;
		}

		internal static IntPtr GetSteamUtils()
		{
			return CSteamGameServerAPIContext.m_pSteamUtils;
		}

		internal static IntPtr GetSteamNetworking()
		{
			return CSteamGameServerAPIContext.m_pSteamNetworking;
		}

		internal static IntPtr GetSteamGameServerStats()
		{
			return CSteamGameServerAPIContext.m_pSteamGameServerStats;
		}

		internal static IntPtr GetSteamHTTP()
		{
			return CSteamGameServerAPIContext.m_pSteamHTTP;
		}

		internal static IntPtr GetSteamInventory()
		{
			return CSteamGameServerAPIContext.m_pSteamInventory;
		}

		internal static IntPtr GetSteamUGC()
		{
			return CSteamGameServerAPIContext.m_pSteamUGC;
		}

		internal static IntPtr GetSteamApps()
		{
			return CSteamGameServerAPIContext.m_pSteamApps;
		}
	}
}

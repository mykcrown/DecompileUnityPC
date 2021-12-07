// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamGameSearch
	{
		public static EGameSearchErrorCode_t AddGameSearchParams(string pchKeyToFind, string pchValuesToFind)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchKeyToFind))
			{
				using (InteropHelp.UTF8StringHandle uTF8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValuesToFind))
				{
					result = NativeMethods.ISteamGameSearch_AddGameSearchParams(CSteamAPIContext.GetSteamGameSearch(), uTF8StringHandle, uTF8StringHandle2);
				}
			}
			return result;
		}

		public static EGameSearchErrorCode_t SearchForGameWithLobby(CSteamID steamIDLobby, int nPlayerMin, int nPlayerMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SearchForGameWithLobby(CSteamAPIContext.GetSteamGameSearch(), steamIDLobby, nPlayerMin, nPlayerMax);
		}

		public static EGameSearchErrorCode_t SearchForGameSolo(int nPlayerMin, int nPlayerMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SearchForGameSolo(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax);
		}

		public static EGameSearchErrorCode_t AcceptGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_AcceptGame(CSteamAPIContext.GetSteamGameSearch());
		}

		public static EGameSearchErrorCode_t DeclineGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_DeclineGame(CSteamAPIContext.GetSteamGameSearch());
		}

		public static EGameSearchErrorCode_t RetrieveConnectionDetails(CSteamID steamIDHost, out string pchConnectionDetails, int cubConnectionDetails)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal(cubConnectionDetails);
			EGameSearchErrorCode_t eGameSearchErrorCode_t = NativeMethods.ISteamGameSearch_RetrieveConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), steamIDHost, intPtr, cubConnectionDetails);
			pchConnectionDetails = ((eGameSearchErrorCode_t == (EGameSearchErrorCode_t)0) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return eGameSearchErrorCode_t;
		}

		public static EGameSearchErrorCode_t EndGameSearch()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_EndGameSearch(CSteamAPIContext.GetSteamGameSearch());
		}

		public static EGameSearchErrorCode_t SetGameHostParams(string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle uTF8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					result = NativeMethods.ISteamGameSearch_SetGameHostParams(CSteamAPIContext.GetSteamGameSearch(), uTF8StringHandle, uTF8StringHandle2);
				}
			}
			return result;
		}

		public static EGameSearchErrorCode_t SetConnectionDetails(string pchConnectionDetails, int cubConnectionDetails)
		{
			InteropHelp.TestIfAvailableClient();
			EGameSearchErrorCode_t result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchConnectionDetails))
			{
				result = NativeMethods.ISteamGameSearch_SetConnectionDetails(CSteamAPIContext.GetSteamGameSearch(), uTF8StringHandle, cubConnectionDetails);
			}
			return result;
		}

		public static EGameSearchErrorCode_t RequestPlayersForGame(int nPlayerMin, int nPlayerMax, int nMaxTeamSize)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_RequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch(), nPlayerMin, nPlayerMax, nMaxTeamSize);
		}

		public static EGameSearchErrorCode_t HostConfirmGameStart(ulong ullUniqueGameID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_HostConfirmGameStart(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
		}

		public static EGameSearchErrorCode_t CancelRequestPlayersForGame()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_CancelRequestPlayersForGame(CSteamAPIContext.GetSteamGameSearch());
		}

		public static EGameSearchErrorCode_t SubmitPlayerResult(ulong ullUniqueGameID, CSteamID steamIDPlayer, EPlayerResult_t EPlayerResult)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_SubmitPlayerResult(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID, steamIDPlayer, EPlayerResult);
		}

		public static EGameSearchErrorCode_t EndGame(ulong ullUniqueGameID)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamGameSearch_EndGame(CSteamAPIContext.GetSteamGameSearch(), ullUniqueGameID);
		}
	}
}

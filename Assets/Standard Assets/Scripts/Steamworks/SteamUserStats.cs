// Decompile from assembly: Assembly-CSharp-firstpass.dll

using System;
using System.Runtime.InteropServices;

namespace Steamworks
{
	public static class SteamUserStats
	{
		public static bool RequestCurrentStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_RequestCurrentStats(CSteamAPIContext.GetSteamUserStats());
		}

		public static bool GetStat(string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetStat(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetStat(string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetStat0(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pData);
			}
			return result;
		}

		public static bool SetStat(string pchName, int nData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetStat(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, nData);
			}
			return result;
		}

		public static bool SetStat(string pchName, float fData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetStat0(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, fData);
			}
			return result;
		}

		public static bool UpdateAvgRateStat(string pchName, float flCountThisSession, double dSessionLength)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_UpdateAvgRateStat(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, flCountThisSession, dSessionLength);
			}
			return result;
		}

		public static bool GetAchievement(string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievement(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pbAchieved);
			}
			return result;
		}

		public static bool SetAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_SetAchievement(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle);
			}
			return result;
		}

		public static bool ClearAchievement(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_ClearAchievement(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle);
			}
			return result;
		}

		public static bool GetAchievementAndUnlockTime(string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementAndUnlockTime(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		public static bool StoreStats()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_StoreStats(CSteamAPIContext.GetSteamUserStats());
		}

		public static int GetAchievementIcon(string pchName)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementIcon(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle);
			}
			return result;
		}

		public static string GetAchievementDisplayAttribute(string pchName, string pchKey)
		{
			InteropHelp.TestIfAvailableClient();
			string result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				using (InteropHelp.UTF8StringHandle uTF8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					result = InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementDisplayAttribute(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, uTF8StringHandle2));
				}
			}
			return result;
		}

		public static bool IndicateAchievementProgress(string pchName, uint nCurProgress, uint nMaxProgress)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_IndicateAchievementProgress(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, nCurProgress, nMaxProgress);
			}
			return result;
		}

		public static uint GetNumAchievements()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetNumAchievements(CSteamAPIContext.GetSteamUserStats());
		}

		public static string GetAchievementName(uint iAchievement)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetAchievementName(CSteamAPIContext.GetSteamUserStats(), iAchievement));
		}

		public static SteamAPICall_t RequestUserStats(CSteamID steamIDUser)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestUserStats(CSteamAPIContext.GetSteamUserStats(), steamIDUser);
		}

		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out int pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserStat(CSteamAPIContext.GetSteamUserStats(), steamIDUser, uTF8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserStat(CSteamID steamIDUser, string pchName, out float pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserStat0(CSteamAPIContext.GetSteamUserStats(), steamIDUser, uTF8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetUserAchievement(CSteamID steamIDUser, string pchName, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserAchievement(CSteamAPIContext.GetSteamUserStats(), steamIDUser, uTF8StringHandle, out pbAchieved);
			}
			return result;
		}

		public static bool GetUserAchievementAndUnlockTime(CSteamID steamIDUser, string pchName, out bool pbAchieved, out uint punUnlockTime)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetUserAchievementAndUnlockTime(CSteamAPIContext.GetSteamUserStats(), steamIDUser, uTF8StringHandle, out pbAchieved, out punUnlockTime);
			}
			return result;
		}

		public static bool ResetAllStats(bool bAchievementsToo)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_ResetAllStats(CSteamAPIContext.GetSteamUserStats(), bAchievementsToo);
		}

		public static SteamAPICall_t FindOrCreateLeaderboard(string pchLeaderboardName, ELeaderboardSortMethod eLeaderboardSortMethod, ELeaderboardDisplayType eLeaderboardDisplayType)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindOrCreateLeaderboard(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, eLeaderboardSortMethod, eLeaderboardDisplayType);
			}
			return result;
		}

		public static SteamAPICall_t FindLeaderboard(string pchLeaderboardName)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchLeaderboardName))
			{
				result = (SteamAPICall_t)NativeMethods.ISteamUserStats_FindLeaderboard(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle);
			}
			return result;
		}

		public static string GetLeaderboardName(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return InteropHelp.PtrToStringUTF8(NativeMethods.ISteamUserStats_GetLeaderboardName(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard));
		}

		public static int GetLeaderboardEntryCount(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardEntryCount(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		public static ELeaderboardSortMethod GetLeaderboardSortMethod(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardSortMethod(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		public static ELeaderboardDisplayType GetLeaderboardDisplayType(SteamLeaderboard_t hSteamLeaderboard)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetLeaderboardDisplayType(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard);
		}

		public static SteamAPICall_t DownloadLeaderboardEntries(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardDataRequest eLeaderboardDataRequest, int nRangeStart, int nRangeEnd)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntries(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, eLeaderboardDataRequest, nRangeStart, nRangeEnd);
		}

		public static SteamAPICall_t DownloadLeaderboardEntriesForUsers(SteamLeaderboard_t hSteamLeaderboard, CSteamID[] prgUsers, int cUsers)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_DownloadLeaderboardEntriesForUsers(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, prgUsers, cUsers);
		}

		public static bool GetDownloadedLeaderboardEntry(SteamLeaderboardEntries_t hSteamLeaderboardEntries, int index, out LeaderboardEntry_t pLeaderboardEntry, int[] pDetails, int cDetailsMax)
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamUserStats_GetDownloadedLeaderboardEntry(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboardEntries, index, out pLeaderboardEntry, pDetails, cDetailsMax);
		}

		public static SteamAPICall_t UploadLeaderboardScore(SteamLeaderboard_t hSteamLeaderboard, ELeaderboardUploadScoreMethod eLeaderboardUploadScoreMethod, int nScore, int[] pScoreDetails, int cScoreDetailsCount)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_UploadLeaderboardScore(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, eLeaderboardUploadScoreMethod, nScore, pScoreDetails, cScoreDetailsCount);
		}

		public static SteamAPICall_t AttachLeaderboardUGC(SteamLeaderboard_t hSteamLeaderboard, UGCHandle_t hUGC)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_AttachLeaderboardUGC(CSteamAPIContext.GetSteamUserStats(), hSteamLeaderboard, hUGC);
		}

		public static SteamAPICall_t GetNumberOfCurrentPlayers()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_GetNumberOfCurrentPlayers(CSteamAPIContext.GetSteamUserStats());
		}

		public static SteamAPICall_t RequestGlobalAchievementPercentages()
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalAchievementPercentages(CSteamAPIContext.GetSteamUserStats());
		}

		public static int GetMostAchievedAchievementInfo(out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetMostAchievedAchievementInfo(CSteamAPIContext.GetSteamUserStats(), intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static int GetNextMostAchievedAchievementInfo(int iIteratorPrevious, out string pchName, uint unNameBufLen, out float pflPercent, out bool pbAchieved)
		{
			InteropHelp.TestIfAvailableClient();
			IntPtr intPtr = Marshal.AllocHGlobal((int)unNameBufLen);
			int num = NativeMethods.ISteamUserStats_GetNextMostAchievedAchievementInfo(CSteamAPIContext.GetSteamUserStats(), iIteratorPrevious, intPtr, unNameBufLen, out pflPercent, out pbAchieved);
			pchName = ((num == -1) ? null : InteropHelp.PtrToStringUTF8(intPtr));
			Marshal.FreeHGlobal(intPtr);
			return num;
		}

		public static bool GetAchievementAchievedPercent(string pchName, out float pflPercent)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchName))
			{
				result = NativeMethods.ISteamUserStats_GetAchievementAchievedPercent(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pflPercent);
			}
			return result;
		}

		public static SteamAPICall_t RequestGlobalStats(int nHistoryDays)
		{
			InteropHelp.TestIfAvailableClient();
			return (SteamAPICall_t)NativeMethods.ISteamUserStats_RequestGlobalStats(CSteamAPIContext.GetSteamUserStats(), nHistoryDays);
		}

		public static bool GetGlobalStat(string pchStatName, out long pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStat(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pData);
			}
			return result;
		}

		public static bool GetGlobalStat(string pchStatName, out double pData)
		{
			InteropHelp.TestIfAvailableClient();
			bool result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStat0(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, out pData);
			}
			return result;
		}

		public static int GetGlobalStatHistory(string pchStatName, long[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStatHistory(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, pData, cubData);
			}
			return result;
		}

		public static int GetGlobalStatHistory(string pchStatName, double[] pData, uint cubData)
		{
			InteropHelp.TestIfAvailableClient();
			int result;
			using (InteropHelp.UTF8StringHandle uTF8StringHandle = new InteropHelp.UTF8StringHandle(pchStatName))
			{
				result = NativeMethods.ISteamUserStats_GetGlobalStatHistory0(CSteamAPIContext.GetSteamUserStats(), uTF8StringHandle, pData, cubData);
			}
			return result;
		}
	}
}

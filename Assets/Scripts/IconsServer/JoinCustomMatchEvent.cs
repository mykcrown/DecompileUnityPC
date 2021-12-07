// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using System;

namespace IconsServer
{
	public class JoinCustomMatchEvent : ServerEvent
	{
		public enum EResult
		{
			Result_Ok,
			Result_InQueue,
			Result_InMatch,
			Result_SystemError,
			Result_TooLate,
			Result_InvalidCreds,
			Result_LobbyFull,
			Result_MatchRunning,
			ResultCount
		}

		public JoinCustomMatchEvent.EResult result;

		public string lobbyName;

		public ulong hostUserId;

		public SBasicMatchPlayerDesc[] players;

		public EIconStages[] stageList;

		public LobbyGameMode gameMode;

		public JoinCustomMatchEvent(JoinCustomMatchEvent.EResult result)
		{
			if (result == JoinCustomMatchEvent.EResult.Result_Ok)
			{
				throw new Exception("Invalid Result. OK result needs to use other constructor");
			}
			this.result = result;
			this.hostUserId = 0uL;
			this.players = null;
			this.stageList = null;
			this.gameMode = LobbyGameMode.Stock;
		}

		public JoinCustomMatchEvent(JoinCustomMatchEvent.EResult result, ulong hostUserId, SBasicMatchPlayerDesc[] players, LobbyGameMode gameMode, EIconStages[] stageList)
		{
			if (result != JoinCustomMatchEvent.EResult.Result_Ok)
			{
				throw new Exception("Invalid Result. error result needs to use other constructor");
			}
			this.result = result;
			this.hostUserId = hostUserId;
			this.players = players;
			this.stageList = stageList;
			this.gameMode = gameMode;
		}
	}
}

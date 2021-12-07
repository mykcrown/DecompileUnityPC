using System;
using MatchMaking;

namespace IconsServer
{
	// Token: 0x020007DC RID: 2012
	public class JoinCustomMatchEvent : ServerEvent
	{
		// Token: 0x060031F8 RID: 12792 RVA: 0x000F24CF File Offset: 0x000F08CF
		public JoinCustomMatchEvent(JoinCustomMatchEvent.EResult result)
		{
			if (result == JoinCustomMatchEvent.EResult.Result_Ok)
			{
				throw new Exception("Invalid Result. OK result needs to use other constructor");
			}
			this.result = result;
			this.hostUserId = 0UL;
			this.players = null;
			this.stageList = null;
			this.gameMode = LobbyGameMode.Stock;
		}

		// Token: 0x060031F9 RID: 12793 RVA: 0x000F250C File Offset: 0x000F090C
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

		// Token: 0x04002302 RID: 8962
		public JoinCustomMatchEvent.EResult result;

		// Token: 0x04002303 RID: 8963
		public string lobbyName;

		// Token: 0x04002304 RID: 8964
		public ulong hostUserId;

		// Token: 0x04002305 RID: 8965
		public SBasicMatchPlayerDesc[] players;

		// Token: 0x04002306 RID: 8966
		public EIconStages[] stageList;

		// Token: 0x04002307 RID: 8967
		public LobbyGameMode gameMode;

		// Token: 0x020007DD RID: 2013
		public enum EResult
		{
			// Token: 0x04002309 RID: 8969
			Result_Ok,
			// Token: 0x0400230A RID: 8970
			Result_InQueue,
			// Token: 0x0400230B RID: 8971
			Result_InMatch,
			// Token: 0x0400230C RID: 8972
			Result_SystemError,
			// Token: 0x0400230D RID: 8973
			Result_TooLate,
			// Token: 0x0400230E RID: 8974
			Result_InvalidCreds,
			// Token: 0x0400230F RID: 8975
			Result_LobbyFull,
			// Token: 0x04002310 RID: 8976
			Result_MatchRunning,
			// Token: 0x04002311 RID: 8977
			ResultCount
		}
	}
}

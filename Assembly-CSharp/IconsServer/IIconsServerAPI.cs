using System;
using System.Collections.Generic;
using MatchMaking;
using network;

namespace IconsServer
{
	// Token: 0x020007DA RID: 2010
	public interface IIconsServerAPI
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060031E6 RID: 12774
		ulong SessionId { get; }

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060031E7 RID: 12775
		ulong AccountId { get; }

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060031E8 RID: 12776
		string Username { get; }

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060031E9 RID: 12777
		int ServerTimestepDelta { get; }

		// Token: 0x060031EA RID: 12778
		bool Startup(int networkPollTimerMs = 10);

		// Token: 0x060031EB RID: 12779
		void Shutdown();

		// Token: 0x060031EC RID: 12780
		void CloseAllConnections();

		// Token: 0x060031ED RID: 12781
		bool LeaveMatch(Guid matchId);

		// Token: 0x060031EE RID: 12782
		bool CustomMatchSetParams(SCustomLobbyParams cmparmas);

		// Token: 0x060031EF RID: 12783
		bool LeaveCustomMatch();

		// Token: 0x060031F0 RID: 12784
		bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode);

		// Token: 0x060031F1 RID: 12785
		bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom);

		// Token: 0x060031F2 RID: 12786
		bool StageLoaded();

		// Token: 0x060031F3 RID: 12787
		bool EnqueueMessage(INetMsg queuedMessage);

		// Token: 0x060031F4 RID: 12788
		bool SendClientWinner(byte winningTeamMask);

		// Token: 0x060031F5 RID: 12789
		void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent;

		// Token: 0x060031F6 RID: 12790
		void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent;
	}
}

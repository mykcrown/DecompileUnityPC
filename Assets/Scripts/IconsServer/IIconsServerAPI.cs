// Decompile from assembly: Assembly-CSharp.dll

using MatchMaking;
using network;
using System;
using System.Collections.Generic;

namespace IconsServer
{
	public interface IIconsServerAPI
	{
		ulong SessionId
		{
			get;
		}

		ulong AccountId
		{
			get;
		}

		string Username
		{
			get;
		}

		int ServerTimestepDelta
		{
			get;
		}

		bool Startup(int networkPollTimerMs = 10);

		void Shutdown();

		void CloseAllConnections();

		bool LeaveMatch(Guid matchId);

		bool CustomMatchSetParams(SCustomLobbyParams cmparmas);

		bool LeaveCustomMatch();

		bool StartCustomMatch(Dictionary<ulong, LobbyPlayerData> players, LobbyGameMode gameMode);

		bool LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom);

		bool StageLoaded();

		bool EnqueueMessage(INetMsg queuedMessage);

		bool SendClientWinner(byte winningTeamMask);

		void ListenForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent;

		void StopListeningForServerEvents<T>(Action<ServerEvent> callback) where T : ServerEvent;
	}
}

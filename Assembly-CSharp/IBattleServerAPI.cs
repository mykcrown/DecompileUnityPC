using System;
using System.Collections.Generic;
using IconsServer;
using MatchMaking;
using P2P;

// Token: 0x020007A3 RID: 1955
public interface IBattleServerAPI
{
	// Token: 0x17000BB4 RID: 2996
	// (get) Token: 0x06003086 RID: 12422
	bool IsConnected { get; }

	// Token: 0x17000BB5 RID: 2997
	// (get) Token: 0x06003087 RID: 12423
	bool IsOnlineMatchReady { get; }

	// Token: 0x17000BB6 RID: 2998
	// (get) Token: 0x06003088 RID: 12424
	bool IsSinglePlayerNetworkGame { get; }

	// Token: 0x17000BB7 RID: 2999
	// (get) Token: 0x06003089 RID: 12425
	int ServerTimestepDelta { get; }

	// Token: 0x17000BB8 RID: 3000
	// (get) Token: 0x0600308A RID: 12426
	ulong ServerPing { get; }

	// Token: 0x17000BB9 RID: 3001
	// (get) Token: 0x0600308B RID: 12427
	Guid MatchID { get; }

	// Token: 0x17000BBA RID: 3002
	// (get) Token: 0x0600308C RID: 12428
	int NumTeams { get; }

	// Token: 0x17000BBB RID: 3003
	// (get) Token: 0x0600308D RID: 12429
	int CurrentMatchIndex { get; }

	// Token: 0x17000BBC RID: 3004
	// (get) Token: 0x0600308E RID: 12430
	int NumMatches { get; }

	// Token: 0x17000BBD RID: 3005
	// (get) Token: 0x0600308F RID: 12431
	List<StageID> Stages { get; }

	// Token: 0x17000BBE RID: 3006
	// (get) Token: 0x06003090 RID: 12432
	List<PlayerNum> LocalPlayerNumIDs { get; }

	// Token: 0x17000BBF RID: 3007
	// (get) Token: 0x06003091 RID: 12433
	PlayerNum GetPrimaryLocalPlayer { get; }

	// Token: 0x17000BC0 RID: 3008
	// (get) Token: 0x06003092 RID: 12434
	int PlayerCount { get; }

	// Token: 0x17000BC1 RID: 3009
	// (get) Token: 0x06003093 RID: 12435
	int NetworkBytesSent { get; }

	// Token: 0x17000BC2 RID: 3010
	// (get) Token: 0x06003094 RID: 12436
	int NetworkBytesReceived { get; }

	// Token: 0x17000BC3 RID: 3011
	// (get) Token: 0x06003095 RID: 12437
	int NumLives { get; }

	// Token: 0x17000BC4 RID: 3012
	// (get) Token: 0x06003096 RID: 12438
	int AssistCount { get; }

	// Token: 0x17000BC5 RID: 3013
	// (get) Token: 0x06003097 RID: 12439
	int MatchTime { get; }

	// Token: 0x17000BC6 RID: 3014
	// (get) Token: 0x06003098 RID: 12440
	GameMode MatchGameMode { get; }

	// Token: 0x17000BC7 RID: 3015
	// (get) Token: 0x06003099 RID: 12441
	GameRules MatchRules { get; }

	// Token: 0x17000BC8 RID: 3016
	// (get) Token: 0x0600309A RID: 12442
	bool TeamAttack { get; }

	// Token: 0x17000BC9 RID: 3017
	// (get) Token: 0x0600309B RID: 12443
	float SelectionTime { get; }

	// Token: 0x17000BCA RID: 3018
	// (get) Token: 0x0600309C RID: 12444
	bool ReceivedMatchResults { get; }

	// Token: 0x17000BCB RID: 3019
	// (get) Token: 0x0600309D RID: 12445
	Dictionary<TeamNum, List<SBasicMatchPlayerDesc>> TeamPlayers { get; }

	// Token: 0x0600309E RID: 12446
	void Initialize();

	// Token: 0x0600309F RID: 12447
	void OnDestroy();

	// Token: 0x060030A0 RID: 12448
	void QueueUnreliableMessage(BatchEvent msg);

	// Token: 0x060030A1 RID: 12449
	void Listen<T>(ServerEventHandler handler) where T : ServerEvent;

	// Token: 0x060030A2 RID: 12450
	void Listen(Type type, ServerEventHandler handler);

	// Token: 0x060030A3 RID: 12451
	void Unsubscribe<T>(ServerEventHandler handler) where T : ServerEvent;

	// Token: 0x060030A4 RID: 12452
	void Unsubscribe(Type type, ServerEventHandler handler);

	// Token: 0x060030A5 RID: 12453
	PlayerNum GetPlayerNum(int index);

	// Token: 0x17000BCC RID: 3020
	// (get) Token: 0x060030A6 RID: 12454
	// (set) Token: 0x060030A7 RID: 12455
	Action<SP2PMatchBasicPlayerDesc[]> OnMatchReady { get; set; }

	// Token: 0x17000BCD RID: 3021
	// (get) Token: 0x060030A8 RID: 12456
	// (set) Token: 0x060030A9 RID: 12457
	Action<bool> OnLeftRoom { get; set; }

	// Token: 0x060030AA RID: 12458
	void StageLoaded();

	// Token: 0x060030AB RID: 12459
	void SendWinner(List<TeamNum> winningTeams);

	// Token: 0x060030AC RID: 12460
	void ClearNetUsage();

	// Token: 0x060030AD RID: 12461
	void LeaveRoom(bool clientExpectsSetToEnd);

	// Token: 0x060030AE RID: 12462
	void ResetRoom();

	// Token: 0x060030AF RID: 12463
	void LockInSelection(ECharacterType characterID, uint characterIndex, ulong skinID, bool isRandom);

	// Token: 0x060030B0 RID: 12464
	bool IsLocalPlayer(PlayerNum playerNum);

	// Token: 0x060030B1 RID: 12465
	bool ResultsForMatch(int MatchIndex);
}

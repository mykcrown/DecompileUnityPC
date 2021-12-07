using System;
using System.Collections.Generic;
using IconsServer;
using UnityEngine;

// Token: 0x0200087D RID: 2173
public class RollbackQuitGame : IRollbackQuitGame
{
	// Token: 0x17000D48 RID: 3400
	// (get) Token: 0x06003694 RID: 13972 RVA: 0x000FF5F5 File Offset: 0x000FD9F5
	// (set) Token: 0x06003695 RID: 13973 RVA: 0x000FF5FD File Offset: 0x000FD9FD
	[Inject]
	public IBattleServerAPI battleServer { private get; set; }

	// Token: 0x17000D49 RID: 3401
	// (get) Token: 0x06003696 RID: 13974 RVA: 0x000FF606 File Offset: 0x000FDA06
	// (set) Token: 0x06003697 RID: 13975 RVA: 0x000FF60E File Offset: 0x000FDA0E
	[Inject]
	public GameController gameController { private get; set; }

	// Token: 0x06003698 RID: 13976 RVA: 0x000FF618 File Offset: 0x000FDA18
	public void Init(IBattleServerAPI battleServerAPI, RollbackSettings settings)
	{
		this.battleServerAPI = battleServerAPI;
		this.playerDataList.Clear();
		this.quitList.Clear();
		foreach (RollbackPlayerData rollbackPlayerData in settings.playerDataList)
		{
			if (rollbackPlayerData.isLocal)
			{
				this.localPlayer = rollbackPlayerData;
			}
		}
		foreach (RollbackPlayerData rollbackPlayerData2 in settings.playerDataList)
		{
			int playerID = rollbackPlayerData2.playerID;
			this.playerDataList[playerID] = rollbackPlayerData2;
		}
		battleServerAPI.Listen<DisconnectAckEvent>(new ServerEventHandler(this.onDisconnectAckMsg));
	}

	// Token: 0x06003699 RID: 13977 RVA: 0x000FF708 File Offset: 0x000FDB08
	public void Destroy()
	{
		this.battleServerAPI.Unsubscribe<DisconnectAckEvent>(new ServerEventHandler(this.onDisconnectAckMsg));
	}

	// Token: 0x0600369A RID: 13978 RVA: 0x000FF724 File Offset: 0x000FDB24
	private void onDisconnectAckMsg(ServerEvent message)
	{
		DisconnectAckEvent disconnectAckEvent = message as DisconnectAckEvent;
		RollbackQuitGame.QuittingData quittingData;
		if (this.activeQuits.TryGetValue(disconnectAckEvent.quittingPlayerID, out quittingData) && quittingData.waitingForAck.Contains(disconnectAckEvent.senderID))
		{
			quittingData.waitingForAck.Remove(disconnectAckEvent.senderID);
		}
	}

	// Token: 0x0600369B RID: 13979 RVA: 0x000FF778 File Offset: 0x000FDB78
	private ulong getUserID(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.userID;
		}
		throw new UnityException("Tried to check status on invalid player " + playerID);
	}

	// Token: 0x0600369C RID: 13980 RVA: 0x000FF7B4 File Offset: 0x000FDBB4
	public void Setup(int playerID, int frame, List<int> players)
	{
		RollbackQuitGame.QuittingData quittingData = new RollbackQuitGame.QuittingData();
		quittingData.playerID = playerID;
		quittingData.frame = frame;
		quittingData.time = WTime.precisionTimeSinceStartup;
		quittingData.waitingForAck = players;
		this.activeQuits[playerID] = quittingData;
		this.quitList[playerID] = true;
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x000FF804 File Offset: 0x000FDC04
	public void Tick()
	{
		foreach (KeyValuePair<int, RollbackQuitGame.QuittingData> keyValuePair in this.activeQuits)
		{
			RollbackQuitGame.QuittingData value = keyValuePair.Value;
			if (value.active)
			{
				if (value.waitingForAck.Count == 0)
				{
					this.complete(value);
				}
				else
				{
					double num = WTime.precisionTimeSinceStartup - value.time;
					if (num >= 5000.0)
					{
						this.complete(value);
					}
					else
					{
						foreach (int playerID in value.waitingForAck)
						{
							DisconnectEvent disconnectEvent = this.disconnectEventBuffer;
							disconnectEvent.frame = value.frame;
							disconnectEvent.playerID = value.playerID;
							disconnectEvent._targetUserID = this.getUserID(playerID);
							this.battleServer.QueueUnreliableMessage(disconnectEvent);
						}
					}
				}
			}
		}
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x000FF93C File Offset: 0x000FDD3C
	private void complete(RollbackQuitGame.QuittingData data)
	{
		data.active = false;
		if (data.playerID == this.localPlayer.playerID)
		{
			int num = 0;
			foreach (RollbackPlayerData rollbackPlayerData in this.playerDataList.Values)
			{
				if (!rollbackPlayerData.isSpectator && rollbackPlayerData.disconnectFrame == -1)
				{
					num++;
				}
			}
			this.gameController.currentGame.ExitGame(ScreenType.CustomLobbyScreen, num);
		}
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x000FF9E4 File Offset: 0x000FDDE4
	public bool IsQuitting(int playerID)
	{
		bool result = false;
		this.quitList.TryGetValue(playerID, out result);
		return result;
	}

	// Token: 0x04002528 RID: 9512
	private const int MAX_WAIT_MS = 5000;

	// Token: 0x0400252B RID: 9515
	private IBattleServerAPI battleServerAPI;

	// Token: 0x0400252C RID: 9516
	private DisconnectEvent disconnectEventBuffer = new DisconnectEvent();

	// Token: 0x0400252D RID: 9517
	private Dictionary<int, RollbackPlayerData> playerDataList = new Dictionary<int, RollbackPlayerData>();

	// Token: 0x0400252E RID: 9518
	private Dictionary<int, bool> quitList = new Dictionary<int, bool>();

	// Token: 0x0400252F RID: 9519
	private RollbackPlayerData localPlayer;

	// Token: 0x04002530 RID: 9520
	private Dictionary<int, RollbackQuitGame.QuittingData> activeQuits = new Dictionary<int, RollbackQuitGame.QuittingData>(8);

	// Token: 0x0200087E RID: 2174
	private class QuittingData
	{
		// Token: 0x04002531 RID: 9521
		public bool active = true;

		// Token: 0x04002532 RID: 9522
		public int playerID;

		// Token: 0x04002533 RID: 9523
		public int frame;

		// Token: 0x04002534 RID: 9524
		public List<int> waitingForAck = new List<int>(8);

		// Token: 0x04002535 RID: 9525
		public double time;
	}
}

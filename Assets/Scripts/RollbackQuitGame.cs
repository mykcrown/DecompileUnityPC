// Decompile from assembly: Assembly-CSharp.dll

using IconsServer;
using System;
using System.Collections.Generic;
using UnityEngine;

public class RollbackQuitGame : IRollbackQuitGame
{
	private class QuittingData
	{
		public bool active = true;

		public int playerID;

		public int frame;

		public List<int> waitingForAck = new List<int>(8);

		public double time;
	}

	private const int MAX_WAIT_MS = 5000;

	private IBattleServerAPI battleServerAPI;

	private DisconnectEvent disconnectEventBuffer = new DisconnectEvent();

	private Dictionary<int, RollbackPlayerData> playerDataList = new Dictionary<int, RollbackPlayerData>();

	private Dictionary<int, bool> quitList = new Dictionary<int, bool>();

	private RollbackPlayerData localPlayer;

	private Dictionary<int, RollbackQuitGame.QuittingData> activeQuits = new Dictionary<int, RollbackQuitGame.QuittingData>(8);

	[Inject]
	public IBattleServerAPI battleServer
	{
		private get;
		set;
	}

	[Inject]
	public GameController gameController
	{
		private get;
		set;
	}

	public void Init(IBattleServerAPI battleServerAPI, RollbackSettings settings)
	{
		this.battleServerAPI = battleServerAPI;
		this.playerDataList.Clear();
		this.quitList.Clear();
		foreach (RollbackPlayerData current in settings.playerDataList)
		{
			if (current.isLocal)
			{
				this.localPlayer = current;
			}
		}
		foreach (RollbackPlayerData current2 in settings.playerDataList)
		{
			int playerID = current2.playerID;
			this.playerDataList[playerID] = current2;
		}
		battleServerAPI.Listen<DisconnectAckEvent>(new ServerEventHandler(this.onDisconnectAckMsg));
	}

	public void Destroy()
	{
		this.battleServerAPI.Unsubscribe<DisconnectAckEvent>(new ServerEventHandler(this.onDisconnectAckMsg));
	}

	private void onDisconnectAckMsg(ServerEvent message)
	{
		DisconnectAckEvent disconnectAckEvent = message as DisconnectAckEvent;
		RollbackQuitGame.QuittingData quittingData;
		if (this.activeQuits.TryGetValue(disconnectAckEvent.quittingPlayerID, out quittingData) && quittingData.waitingForAck.Contains(disconnectAckEvent.senderID))
		{
			quittingData.waitingForAck.Remove(disconnectAckEvent.senderID);
		}
	}

	private ulong getUserID(int playerID)
	{
		RollbackPlayerData rollbackPlayerData;
		if (this.playerDataList.TryGetValue(playerID, out rollbackPlayerData))
		{
			return rollbackPlayerData.userID;
		}
		throw new UnityException("Tried to check status on invalid player " + playerID);
	}

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

	public void Tick()
	{
		foreach (KeyValuePair<int, RollbackQuitGame.QuittingData> current in this.activeQuits)
		{
			RollbackQuitGame.QuittingData value = current.Value;
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
						foreach (int current2 in value.waitingForAck)
						{
							DisconnectEvent disconnectEvent = this.disconnectEventBuffer;
							disconnectEvent.frame = value.frame;
							disconnectEvent.playerID = value.playerID;
							disconnectEvent._targetUserID = this.getUserID(current2);
							this.battleServer.QueueUnreliableMessage(disconnectEvent);
						}
					}
				}
			}
		}
	}

	private void complete(RollbackQuitGame.QuittingData data)
	{
		data.active = false;
		if (data.playerID == this.localPlayer.playerID)
		{
			int num = 0;
			foreach (RollbackPlayerData current in this.playerDataList.Values)
			{
				if (!current.isSpectator && current.disconnectFrame == -1)
				{
					num++;
				}
			}
			this.gameController.currentGame.ExitGame(ScreenType.CustomLobbyScreen, num);
		}
	}

	public bool IsQuitting(int playerID)
	{
		bool result = false;
		this.quitList.TryGetValue(playerID, out result);
		return result;
	}
}

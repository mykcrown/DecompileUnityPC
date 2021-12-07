// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class OnlineBlindPickPlayerOrganizer : MonoBehaviour
{
	private class PlayerPositionInfo
	{
		public float localScale;

		public Vector3 localOffset;
	}

	public GameObject PlayerList;

	public PlayerSelectionUI PlayerPortraitPrefab;

	public HorizontalLayoutGroup Aligner;

	private Dictionary<PlayerNum, PlayerSelectionUI> playerPortraits = new Dictionary<PlayerNum, PlayerSelectionUI>();

	private Dictionary<int, Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>> NumPlayersToPlayerInfo;

	[Inject]
	public CharacterSelectCalculator characterSelectCalculator
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI
	{
		get;
		set;
	}

	[Inject]
	public IEnterNewGame enterNewGame
	{
		get;
		set;
	}

	[Inject]
	public IOnlineBlindPickScreenAPI onlineBlindPickScreenAPI
	{
		get;
		set;
	}

	public void Setup(GameModeData modeData, Dictionary<ulong, LobbyPlayerData> players)
	{
	}

	public void Setup(GameModeData modeData, List<PlayerNum> playerNums)
	{
		if (this.NumPlayersToPlayerInfo == null)
		{
			this.NumPlayersToPlayerInfo = new Dictionary<int, Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>>();
			this.NumPlayersToPlayerInfo[1] = new Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>();
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo.localScale = 1.6f;
			playerPositionInfo.localOffset = new Vector3(0f, 0f, 0.5f);
			this.NumPlayersToPlayerInfo[1][0] = playerPositionInfo;
			this.NumPlayersToPlayerInfo[2] = new Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>();
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo2 = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo2.localScale = 1.3f;
			playerPositionInfo2.localOffset = new Vector3(-130f, 0f, 0.5f);
			this.NumPlayersToPlayerInfo[2][0] = playerPositionInfo2;
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo3 = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo3.localScale = 0.8f;
			playerPositionInfo3.localOffset = new Vector3(200f, 50f, 0f);
			this.NumPlayersToPlayerInfo[2][1] = playerPositionInfo3;
			this.NumPlayersToPlayerInfo[3] = new Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>();
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo4 = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo4.localScale = 1.1f;
			playerPositionInfo4.localOffset = new Vector3(0f, 0f, 0.5f);
			this.NumPlayersToPlayerInfo[3][0] = playerPositionInfo4;
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo5 = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo5.localScale = 0.7f;
			playerPositionInfo5.localOffset = new Vector3(-325f, 50f, 0f);
			this.NumPlayersToPlayerInfo[3][1] = playerPositionInfo5;
			OnlineBlindPickPlayerOrganizer.PlayerPositionInfo playerPositionInfo6 = new OnlineBlindPickPlayerOrganizer.PlayerPositionInfo();
			playerPositionInfo6.localScale = 0.7f;
			playerPositionInfo6.localOffset = new Vector3(325f, 50f, 0f);
			this.NumPlayersToPlayerInfo[3][2] = playerPositionInfo6;
		}
		this.playerPortraits.Clear();
		for (int i = 0; i < playerNums.Count; i++)
		{
			PlayerNum playerNum = playerNums[i];
			if (PlayerUtil.IsValidPlayer(playerNum))
			{
				PlayerSelectionInfo playerSelectionInfo = this.enterNewGame.GamePayload.FindPlayerInfo(playerNum);
				PlayerSelectionUI playerSelectionUI = UnityEngine.Object.Instantiate<PlayerSelectionUI>(this.PlayerPortraitPrefab, this.PlayerList.transform, false);
				playerSelectionUI.gameObject.SetActive(true);
				playerSelectionUI.IsDisplayed = true;
				this.playerPortraits.Add(playerNum, playerSelectionUI);
				if (playerSelectionInfo == null)
				{
					playerSelectionInfo = new PlayerSelectionInfo();
					playerSelectionInfo.playerNum = playerNum;
				}
				playerSelectionUI.Initialize(playerSelectionInfo, modeData, true);
				PlayerSelectionUI expr_268 = playerSelectionUI;
				expr_268.ReadyClicked = (Action)Delegate.Combine(expr_268.ReadyClicked, new Action(this._Setup_m__0));
			}
		}
	}

	public bool OnCancelPressed(IPlayerCursor cursor)
	{
		using (Dictionary<PlayerNum, PlayerSelectionUI>.ValueCollection.Enumerator enumerator = this.playerPortraits.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				PlayerSelectionUI current = enumerator.Current;
				return current.OnCancelPressed();
			}
		}
		return false;
	}

	public void UpdatePayload()
	{
		foreach (PlayerSelectionUI current in this.playerPortraits.Values)
		{
			current.UpdatePayload();
		}
	}

	public void LockedIn(bool locked)
	{
		foreach (PlayerSelectionUI current in this.playerPortraits.Values)
		{
			current.LockedIn(locked);
		}
	}

	private void _Setup_m__0()
	{
		this.onlineBlindPickScreenAPI.LockInSelection();
	}
}

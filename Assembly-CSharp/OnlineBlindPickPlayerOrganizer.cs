using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020009B2 RID: 2482
public class OnlineBlindPickPlayerOrganizer : MonoBehaviour
{
	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x0600449D RID: 17565 RVA: 0x0012DCD4 File Offset: 0x0012C0D4
	// (set) Token: 0x0600449E RID: 17566 RVA: 0x0012DCDC File Offset: 0x0012C0DC
	[Inject]
	public CharacterSelectCalculator characterSelectCalculator { get; set; }

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x0600449F RID: 17567 RVA: 0x0012DCE5 File Offset: 0x0012C0E5
	// (set) Token: 0x060044A0 RID: 17568 RVA: 0x0012DCED File Offset: 0x0012C0ED
	[Inject]
	public IBattleServerStagingAPI battleServerStagingAPI { get; set; }

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x060044A1 RID: 17569 RVA: 0x0012DCF6 File Offset: 0x0012C0F6
	// (set) Token: 0x060044A2 RID: 17570 RVA: 0x0012DCFE File Offset: 0x0012C0FE
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x060044A3 RID: 17571 RVA: 0x0012DD07 File Offset: 0x0012C107
	// (set) Token: 0x060044A4 RID: 17572 RVA: 0x0012DD0F File Offset: 0x0012C10F
	[Inject]
	public IOnlineBlindPickScreenAPI onlineBlindPickScreenAPI { get; set; }

	// Token: 0x060044A5 RID: 17573 RVA: 0x0012DD18 File Offset: 0x0012C118
	public void Setup(GameModeData modeData, Dictionary<ulong, LobbyPlayerData> players)
	{
	}

	// Token: 0x060044A6 RID: 17574 RVA: 0x0012DD1C File Offset: 0x0012C11C
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
				PlayerSelectionUI playerSelectionUI2 = playerSelectionUI;
				playerSelectionUI2.ReadyClicked = (Action)Delegate.Combine(playerSelectionUI2.ReadyClicked, new Action(delegate()
				{
					this.onlineBlindPickScreenAPI.LockInSelection();
				}));
			}
		}
	}

	// Token: 0x060044A7 RID: 17575 RVA: 0x0012DFC8 File Offset: 0x0012C3C8
	public bool OnCancelPressed(IPlayerCursor cursor)
	{
		using (Dictionary<PlayerNum, PlayerSelectionUI>.ValueCollection.Enumerator enumerator = this.playerPortraits.Values.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				PlayerSelectionUI playerSelectionUI = enumerator.Current;
				return playerSelectionUI.OnCancelPressed();
			}
		}
		return false;
	}

	// Token: 0x060044A8 RID: 17576 RVA: 0x0012E034 File Offset: 0x0012C434
	public void UpdatePayload()
	{
		foreach (PlayerSelectionUI playerSelectionUI in this.playerPortraits.Values)
		{
			playerSelectionUI.UpdatePayload();
		}
	}

	// Token: 0x060044A9 RID: 17577 RVA: 0x0012E094 File Offset: 0x0012C494
	public void LockedIn(bool locked)
	{
		foreach (PlayerSelectionUI playerSelectionUI in this.playerPortraits.Values)
		{
			playerSelectionUI.LockedIn(locked);
		}
	}

	// Token: 0x04002DAC RID: 11692
	public GameObject PlayerList;

	// Token: 0x04002DAD RID: 11693
	public PlayerSelectionUI PlayerPortraitPrefab;

	// Token: 0x04002DAE RID: 11694
	public HorizontalLayoutGroup Aligner;

	// Token: 0x04002DAF RID: 11695
	private Dictionary<PlayerNum, PlayerSelectionUI> playerPortraits = new Dictionary<PlayerNum, PlayerSelectionUI>();

	// Token: 0x04002DB0 RID: 11696
	private Dictionary<int, Dictionary<int, OnlineBlindPickPlayerOrganizer.PlayerPositionInfo>> NumPlayersToPlayerInfo;

	// Token: 0x020009B3 RID: 2483
	private class PlayerPositionInfo
	{
		// Token: 0x04002DB1 RID: 11697
		public float localScale;

		// Token: 0x04002DB2 RID: 11698
		public Vector3 localOffset;
	}
}

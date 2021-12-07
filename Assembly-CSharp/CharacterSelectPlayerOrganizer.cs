using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020008DD RID: 2269
public class CharacterSelectPlayerOrganizer : MonoBehaviour
{
	// Token: 0x17000DDA RID: 3546
	// (get) Token: 0x06003984 RID: 14724 RVA: 0x0010D209 File Offset: 0x0010B609
	// (set) Token: 0x06003985 RID: 14725 RVA: 0x0010D211 File Offset: 0x0010B611
	[Inject]
	public CharacterSelectCalculator characterSelectCalculator { get; set; }

	// Token: 0x17000DDB RID: 3547
	// (get) Token: 0x06003986 RID: 14726 RVA: 0x0010D21A File Offset: 0x0010B61A
	// (set) Token: 0x06003987 RID: 14727 RVA: 0x0010D222 File Offset: 0x0010B622
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x06003988 RID: 14728 RVA: 0x0010D22C File Offset: 0x0010B62C
	public void UpdatePayload()
	{
		foreach (PlayerSelectionUI playerSelectionUI in this.playerPortraits.Values)
		{
			playerSelectionUI.UpdatePayload();
		}
		this.updatePlayers(false);
	}

	// Token: 0x06003989 RID: 14729 RVA: 0x0010D294 File Offset: 0x0010B694
	public bool OnCancelPressed(IPlayerCursor cursor)
	{
		PlayerNum playerNumFromInt = PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false);
		return this.playerPortraits.ContainsKey(playerNumFromInt) && this.playerPortraits[playerNumFromInt].OnCancelPressed();
	}

	// Token: 0x0600398A RID: 14730 RVA: 0x0010D2D4 File Offset: 0x0010B6D4
	public void Setup(GameModeData modeData)
	{
		this.originalPaddingLeft = (float)this.Aligner.padding.left;
		this.originalPaddingRight = (float)this.Aligner.padding.right;
		foreach (PlayerNum playerNum in EnumUtil.GetValues<PlayerNum>())
		{
			if (PlayerUtil.IsValidPlayer(playerNum))
			{
				PlayerSelectionInfo playerSelectionInfo = this.enterNewGame.GamePayload.FindPlayerInfo(playerNum);
				PlayerSelectionUI playerSelectionUI = UnityEngine.Object.Instantiate<PlayerSelectionUI>(this.PlayerPortraitPrefab);
				playerSelectionUI.transform.SetParent(base.transform, false);
				playerSelectionUI.gameObject.SetActive(false);
				this.playerPortraits.Add(playerNum, playerSelectionUI);
				if (playerSelectionInfo == null)
				{
					playerSelectionInfo = new PlayerSelectionInfo();
					playerSelectionInfo.playerNum = playerNum;
				}
				playerSelectionUI.Initialize(playerSelectionInfo, modeData, false);
			}
		}
		this.updatePlayers(true);
	}

	// Token: 0x0600398B RID: 14731 RVA: 0x0010D3B0 File Offset: 0x0010B7B0
	private void updatePlayers(bool initialize = false)
	{
		int maxPlayers = this.getMaxPlayers();
		if (maxPlayers != this.prevMaxPlayers || initialize)
		{
			this.prepareTransitionLayout();
			this.updateAligner();
			Dictionary<PlayerNum, bool> showPlayers = this.getShowPlayers();
			Dictionary<PlayerNum, bool> dictionary = new Dictionary<PlayerNum, bool>();
			foreach (PlayerNum key in this.playerPortraits.Keys)
			{
				PlayerSelectionUI playerSelectionUI = this.playerPortraits[key];
				dictionary[key] = playerSelectionUI.IsDisplayed;
				playerSelectionUI.IsDisplayed = showPlayers[key];
			}
			if (initialize)
			{
				this.updatePlayersStatic(showPlayers);
			}
			else
			{
				List<PlayerSelectionUI> playerUIList = this.getPlayerUIList(showPlayers);
				List<PlayerSelectionUI> playerUIList2 = this.getPlayerUIList(dictionary);
				if (maxPlayers > this.prevMaxPlayers)
				{
					Dictionary<PlayerSelectionUI, Vector3> targetPositions = this.getTargetPositions(playerUIList, this.Aligner);
					this.setInitialPositions(playerUIList, playerUIList2);
					foreach (PlayerSelectionUI playerSelectionUI2 in playerUIList)
					{
						playerSelectionUI2.TweenIn(0.25f, targetPositions[playerSelectionUI2]);
					}
				}
				else
				{
					Dictionary<PlayerSelectionUI, Vector3> targetPositions2 = this.getTargetPositions(playerUIList2, this.ExtendedAligner);
					foreach (PlayerSelectionUI playerSelectionUI3 in playerUIList2)
					{
						playerSelectionUI3.TweenOut(0.25f, targetPositions2[playerSelectionUI3], playerSelectionUI3.IsDisplayed);
					}
				}
			}
			this.prevMaxPlayers = maxPlayers;
		}
	}

	// Token: 0x0600398C RID: 14732 RVA: 0x0010D584 File Offset: 0x0010B984
	private void updateAligner()
	{
		float num = 4f;
		int maxPlayers = this.getMaxPlayers();
		float num2 = (float)maxPlayers / num;
		RectOffset padding = this.Aligner.padding;
		padding.left = (int)(this.originalPaddingLeft / num2);
		padding.right = (int)(this.originalPaddingRight / num2);
		this.Aligner.padding = padding;
	}

	// Token: 0x0600398D RID: 14733 RVA: 0x0010D5D9 File Offset: 0x0010B9D9
	private int getMaxPlayers()
	{
		return this.characterSelectCalculator.GetMaxPlayers();
	}

	// Token: 0x0600398E RID: 14734 RVA: 0x0010D5E8 File Offset: 0x0010B9E8
	private void setInitialPositions(List<PlayerSelectionUI> willShow, List<PlayerSelectionUI> wasShown)
	{
		int maxPlayers = this.getMaxPlayers();
		Dictionary<PlayerSelectionUI, Vector3> dictionary = new Dictionary<PlayerSelectionUI, Vector3>();
		Dictionary<PlayerSelectionUI, Transform> dictionary2 = new Dictionary<PlayerSelectionUI, Transform>();
		foreach (PlayerSelectionUI playerSelectionUI in wasShown)
		{
			playerSelectionUI.gameObject.SetActive(true);
			dictionary[playerSelectionUI] = playerSelectionUI.transform.localPosition;
			dictionary2[playerSelectionUI] = playerSelectionUI.transform.parent;
		}
		foreach (PlayerSelectionUI playerSelectionUI2 in willShow)
		{
			playerSelectionUI2.transform.SetParent(this.ExtendedAligner.transform, false);
		}
		this.ExtendedAligner.Redraw();
		int num = 0;
		foreach (PlayerSelectionUI playerSelectionUI3 in willShow)
		{
			if (!wasShown.Contains(playerSelectionUI3))
			{
				if (num < maxPlayers / 2)
				{
					playerSelectionUI3.transform.SetParent(this.PlayerListLeft.transform, true);
				}
				else
				{
					playerSelectionUI3.transform.SetParent(this.PlayerListRight.transform, true);
				}
			}
			num++;
		}
		foreach (PlayerSelectionUI playerSelectionUI4 in wasShown)
		{
			playerSelectionUI4.transform.SetParent(dictionary2[playerSelectionUI4], false);
			playerSelectionUI4.transform.localPosition = dictionary[playerSelectionUI4];
		}
	}

	// Token: 0x0600398F RID: 14735 RVA: 0x0010D7E0 File Offset: 0x0010BBE0
	private Dictionary<PlayerSelectionUI, Vector3> getTargetPositions(List<PlayerSelectionUI> guiList, HorizontalLayoutGroup aligner)
	{
		Dictionary<PlayerSelectionUI, Vector3> dictionary = new Dictionary<PlayerSelectionUI, Vector3>();
		Dictionary<PlayerSelectionUI, Transform> dictionary2 = new Dictionary<PlayerSelectionUI, Transform>();
		foreach (PlayerSelectionUI playerSelectionUI in guiList)
		{
			playerSelectionUI.gameObject.SetActive(true);
			dictionary[playerSelectionUI] = playerSelectionUI.transform.localPosition;
			dictionary2[playerSelectionUI] = playerSelectionUI.transform.parent;
		}
		foreach (PlayerSelectionUI playerSelectionUI2 in guiList)
		{
			playerSelectionUI2.transform.SetParent(aligner.transform, false);
		}
		aligner.Redraw();
		Dictionary<PlayerSelectionUI, Vector3> dictionary3 = new Dictionary<PlayerSelectionUI, Vector3>();
		foreach (PlayerSelectionUI playerSelectionUI3 in guiList)
		{
			dictionary3[playerSelectionUI3] = playerSelectionUI3.transform.position;
		}
		foreach (PlayerSelectionUI playerSelectionUI4 in guiList)
		{
			playerSelectionUI4.transform.SetParent(dictionary2[playerSelectionUI4], false);
			playerSelectionUI4.transform.localPosition = dictionary[playerSelectionUI4];
		}
		return dictionary3;
	}

	// Token: 0x06003990 RID: 14736 RVA: 0x0010D990 File Offset: 0x0010BD90
	private void updatePlayersStatic(Dictionary<PlayerNum, bool> showPlayers)
	{
		int maxPlayers = this.getMaxPlayers();
		foreach (PlayerNum key in showPlayers.Keys)
		{
			PlayerSelectionUI playerSelectionUI = this.playerPortraits[key];
			if (showPlayers[key])
			{
				playerSelectionUI.transform.SetParent(this.Aligner.transform, false);
				playerSelectionUI.gameObject.SetActive(true);
				playerSelectionUI.IsDisplayed = true;
			}
			else
			{
				playerSelectionUI.gameObject.SetActive(false);
				playerSelectionUI.IsDisplayed = false;
			}
		}
		this.Aligner.Redraw();
		int num = 0;
		foreach (PlayerNum key2 in showPlayers.Keys)
		{
			if (showPlayers[key2])
			{
				PlayerSelectionUI playerSelectionUI2 = this.playerPortraits[key2];
				if (num < maxPlayers / 2)
				{
					playerSelectionUI2.transform.SetParent(this.PlayerListLeft.transform, true);
				}
				else
				{
					playerSelectionUI2.transform.SetParent(this.PlayerListRight.transform, true);
				}
				num++;
			}
		}
	}

	// Token: 0x06003991 RID: 14737 RVA: 0x0010DAFC File Offset: 0x0010BEFC
	private void prepareTransitionLayout()
	{
		int maxPlayers = this.getMaxPlayers();
		if (maxPlayers != this.prevMaxPlayers && this.prevMaxPlayers > 0)
		{
			float num = (float)Mathf.Max(maxPlayers, this.prevMaxPlayers);
			float num2 = (float)Mathf.Min(maxPlayers, this.prevMaxPlayers);
			float num3 = num / num2;
			float x = this.Aligner.GetComponent<RectTransform>().sizeDelta.x;
			float x2 = x * num3;
			RectTransform component = this.ExtendedAligner.GetComponent<RectTransform>();
			Vector2 sizeDelta = component.sizeDelta;
			sizeDelta.x = x2;
			component.sizeDelta = sizeDelta;
			RectOffset padding = this.ExtendedAligner.padding;
			padding.left = (int)(this.originalPaddingLeft / num3);
			padding.right = (int)(this.originalPaddingRight * num3);
			this.ExtendedAligner.padding = padding;
		}
	}

	// Token: 0x06003992 RID: 14738 RVA: 0x0010DBCC File Offset: 0x0010BFCC
	private Dictionary<PlayerNum, bool> getShowPlayers()
	{
		int maxPlayers = this.getMaxPlayers();
		Dictionary<PlayerNum, bool> dictionary = new Dictionary<PlayerNum, bool>();
		foreach (PlayerNum key in this.playerPortraits.Keys)
		{
			dictionary[key] = false;
		}
		for (int i = 0; i < maxPlayers; i++)
		{
			PlayerNum playerNumFromInt = PlayerUtil.GetPlayerNumFromInt(i + 1, false);
			dictionary[playerNumFromInt] = true;
		}
		return dictionary;
	}

	// Token: 0x06003993 RID: 14739 RVA: 0x0010DC68 File Offset: 0x0010C068
	private List<PlayerSelectionUI> getPlayerUIList(Dictionary<PlayerNum, bool> activePlayers)
	{
		List<PlayerSelectionUI> list = new List<PlayerSelectionUI>();
		foreach (PlayerNum key in activePlayers.Keys)
		{
			PlayerSelectionUI item = this.playerPortraits[key];
			if (activePlayers[key])
			{
				list.Add(item);
			}
		}
		return list;
	}

	// Token: 0x040027AD RID: 10157
	public GameObject PlayerListLeft;

	// Token: 0x040027AE RID: 10158
	public GameObject PlayerListRight;

	// Token: 0x040027AF RID: 10159
	public HorizontalLayoutGroup Aligner;

	// Token: 0x040027B0 RID: 10160
	public HorizontalLayoutGroup ExtendedAligner;

	// Token: 0x040027B1 RID: 10161
	public PlayerSelectionUI PlayerPortraitPrefab;

	// Token: 0x040027B2 RID: 10162
	private float originalPaddingLeft;

	// Token: 0x040027B3 RID: 10163
	private float originalPaddingRight;

	// Token: 0x040027B4 RID: 10164
	private int prevMaxPlayers;

	// Token: 0x040027B5 RID: 10165
	private Dictionary<PlayerNum, PlayerSelectionUI> playerPortraits = new Dictionary<PlayerNum, PlayerSelectionUI>();
}

// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectPlayerOrganizer : MonoBehaviour
{
	public GameObject PlayerListLeft;

	public GameObject PlayerListRight;

	public HorizontalLayoutGroup Aligner;

	public HorizontalLayoutGroup ExtendedAligner;

	public PlayerSelectionUI PlayerPortraitPrefab;

	private float originalPaddingLeft;

	private float originalPaddingRight;

	private int prevMaxPlayers;

	private Dictionary<PlayerNum, PlayerSelectionUI> playerPortraits = new Dictionary<PlayerNum, PlayerSelectionUI>();

	[Inject]
	public CharacterSelectCalculator characterSelectCalculator
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

	public void UpdatePayload()
	{
		foreach (PlayerSelectionUI current in this.playerPortraits.Values)
		{
			current.UpdatePayload();
		}
		this.updatePlayers(false);
	}

	public bool OnCancelPressed(IPlayerCursor cursor)
	{
		PlayerNum playerNumFromInt = PlayerUtil.GetPlayerNumFromInt(cursor.PointerId, false);
		return this.playerPortraits.ContainsKey(playerNumFromInt) && this.playerPortraits[playerNumFromInt].OnCancelPressed();
	}

	public void Setup(GameModeData modeData)
	{
		this.originalPaddingLeft = (float)this.Aligner.padding.left;
		this.originalPaddingRight = (float)this.Aligner.padding.right;
		PlayerNum[] values = EnumUtil.GetValues<PlayerNum>();
		for (int i = 0; i < values.Length; i++)
		{
			PlayerNum playerNum = values[i];
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

	private void updatePlayers(bool initialize = false)
	{
		int maxPlayers = this.getMaxPlayers();
		if (maxPlayers != this.prevMaxPlayers || initialize)
		{
			this.prepareTransitionLayout();
			this.updateAligner();
			Dictionary<PlayerNum, bool> showPlayers = this.getShowPlayers();
			Dictionary<PlayerNum, bool> dictionary = new Dictionary<PlayerNum, bool>();
			foreach (PlayerNum current in this.playerPortraits.Keys)
			{
				PlayerSelectionUI playerSelectionUI = this.playerPortraits[current];
				dictionary[current] = playerSelectionUI.IsDisplayed;
				playerSelectionUI.IsDisplayed = showPlayers[current];
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
					foreach (PlayerSelectionUI current2 in playerUIList)
					{
						current2.TweenIn(0.25f, targetPositions[current2]);
					}
				}
				else
				{
					Dictionary<PlayerSelectionUI, Vector3> targetPositions2 = this.getTargetPositions(playerUIList2, this.ExtendedAligner);
					foreach (PlayerSelectionUI current3 in playerUIList2)
					{
						current3.TweenOut(0.25f, targetPositions2[current3], current3.IsDisplayed);
					}
				}
			}
			this.prevMaxPlayers = maxPlayers;
		}
	}

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

	private int getMaxPlayers()
	{
		return this.characterSelectCalculator.GetMaxPlayers();
	}

	private void setInitialPositions(List<PlayerSelectionUI> willShow, List<PlayerSelectionUI> wasShown)
	{
		int maxPlayers = this.getMaxPlayers();
		Dictionary<PlayerSelectionUI, Vector3> dictionary = new Dictionary<PlayerSelectionUI, Vector3>();
		Dictionary<PlayerSelectionUI, Transform> dictionary2 = new Dictionary<PlayerSelectionUI, Transform>();
		foreach (PlayerSelectionUI current in wasShown)
		{
			current.gameObject.SetActive(true);
			dictionary[current] = current.transform.localPosition;
			dictionary2[current] = current.transform.parent;
		}
		foreach (PlayerSelectionUI current2 in willShow)
		{
			current2.transform.SetParent(this.ExtendedAligner.transform, false);
		}
		this.ExtendedAligner.Redraw();
		int num = 0;
		foreach (PlayerSelectionUI current3 in willShow)
		{
			if (!wasShown.Contains(current3))
			{
				if (num < maxPlayers / 2)
				{
					current3.transform.SetParent(this.PlayerListLeft.transform, true);
				}
				else
				{
					current3.transform.SetParent(this.PlayerListRight.transform, true);
				}
			}
			num++;
		}
		foreach (PlayerSelectionUI current4 in wasShown)
		{
			current4.transform.SetParent(dictionary2[current4], false);
			current4.transform.localPosition = dictionary[current4];
		}
	}

	private Dictionary<PlayerSelectionUI, Vector3> getTargetPositions(List<PlayerSelectionUI> guiList, HorizontalLayoutGroup aligner)
	{
		Dictionary<PlayerSelectionUI, Vector3> dictionary = new Dictionary<PlayerSelectionUI, Vector3>();
		Dictionary<PlayerSelectionUI, Transform> dictionary2 = new Dictionary<PlayerSelectionUI, Transform>();
		foreach (PlayerSelectionUI current in guiList)
		{
			current.gameObject.SetActive(true);
			dictionary[current] = current.transform.localPosition;
			dictionary2[current] = current.transform.parent;
		}
		foreach (PlayerSelectionUI current2 in guiList)
		{
			current2.transform.SetParent(aligner.transform, false);
		}
		aligner.Redraw();
		Dictionary<PlayerSelectionUI, Vector3> dictionary3 = new Dictionary<PlayerSelectionUI, Vector3>();
		foreach (PlayerSelectionUI current3 in guiList)
		{
			dictionary3[current3] = current3.transform.position;
		}
		foreach (PlayerSelectionUI current4 in guiList)
		{
			current4.transform.SetParent(dictionary2[current4], false);
			current4.transform.localPosition = dictionary[current4];
		}
		return dictionary3;
	}

	private void updatePlayersStatic(Dictionary<PlayerNum, bool> showPlayers)
	{
		int maxPlayers = this.getMaxPlayers();
		foreach (PlayerNum current in showPlayers.Keys)
		{
			PlayerSelectionUI playerSelectionUI = this.playerPortraits[current];
			if (showPlayers[current])
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
		foreach (PlayerNum current2 in showPlayers.Keys)
		{
			if (showPlayers[current2])
			{
				PlayerSelectionUI playerSelectionUI2 = this.playerPortraits[current2];
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

	private Dictionary<PlayerNum, bool> getShowPlayers()
	{
		int maxPlayers = this.getMaxPlayers();
		Dictionary<PlayerNum, bool> dictionary = new Dictionary<PlayerNum, bool>();
		foreach (PlayerNum current in this.playerPortraits.Keys)
		{
			dictionary[current] = false;
		}
		for (int i = 0; i < maxPlayers; i++)
		{
			PlayerNum playerNumFromInt = PlayerUtil.GetPlayerNumFromInt(i + 1, false);
			dictionary[playerNumFromInt] = true;
		}
		return dictionary;
	}

	private List<PlayerSelectionUI> getPlayerUIList(Dictionary<PlayerNum, bool> activePlayers)
	{
		List<PlayerSelectionUI> list = new List<PlayerSelectionUI>();
		foreach (PlayerNum current in activePlayers.Keys)
		{
			PlayerSelectionUI item = this.playerPortraits[current];
			if (activePlayers[current])
			{
				list.Add(item);
			}
		}
		return list;
	}
}

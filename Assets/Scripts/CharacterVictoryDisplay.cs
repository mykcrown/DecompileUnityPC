// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterVictoryDisplay : MonoBehaviour
{
	public UIColorContainer textColors;

	public ColorSpriteContainer PortraitBackgroundSprites;

	public Image Background;

	public Image PortraitBackground;

	public Image Portrait;

	public TextMeshProUGUI PlayerNameText;

	public GameObject WinnerText;

	public GameObject StatLinePrefab;

	public Transform Tray;

	public Sprite WinningBgSprite;

	public Transform NetsukeLeft;

	public Transform NetsukeRight;

	public Transform NetsukeBottom;

	public float netsukeLeftRightScale = 1.15f;

	public float netsukeBottomScale = 0.65f;

	private VictoryScene3D victoryScene;

	private SkinData skinData;

	private int index;

	[Inject]
	public GameDataManager gameData
	{
		private get;
		set;
	}

	[Inject]
	public ILocalization localization
	{
		private get;
		set;
	}

	[Inject]
	public IUIAdapter uiAdapter
	{
		private get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		private get;
		set;
	}

	[Inject]
	public IVictoryScreenAPI api
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		private get;
		set;
	}

	[Inject]
	public ISkinDataManager skinDataManager
	{
		private get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		private get;
		set;
	}

	[Inject]
	public GameDataManager gameDataManager
	{
		private get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		private get;
		set;
	}

	public void Init(PlayerStats stats, int index, GameModeData gameModeData, SkinDefinition skin, bool isVictor, string playerName, string characterName, int totalPlayers)
	{
		this.index = index;
		this.victoryScene = this.uiAdapter.GetUIScene<VictoryScene3D>();
		UIColor uIColor = PlayerUtil.GetUIColor(stats.playerInfo, gameModeData.settings.usesTeams);
		this.PlayerNameText.text = playerName;
		this.PlayerNameText.color = this.textColors.GetColor(uIColor);
		this.skinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(stats.playerInfo.characterID, skin.uniqueKey));
		this.Portrait.sprite = this.skinData.battlePortrait;
		this.updatePortraitPosition();
		this.PortraitBackground.sprite = this.PortraitBackgroundSprites.GetSprite(uIColor);
		this.WinnerText.SetActive(isVictor);
		if (isVictor)
		{
			this.Background.sprite = this.WinningBgSprite;
		}
		this.addStat(StatType.Kill, stats);
		this.addStat(StatType.Suicide, stats);
		this.addStat(StatType.Death, stats);
		int stat = stats.GetStat(StatType.MaxRound);
		if (stat > 0)
		{
			this.addStat(StatType.MaxRound, stats);
		}
		this.addNetsukeView(stats, totalPlayers);
	}

	private void updatePortraitPosition()
	{
		if (this.skinData.overrideVictoryPortraitOffset)
		{
			Vector3 localPosition = new Vector3((float)this.skinData.victoryPortraitOffset.x, (float)this.skinData.victoryPortraitOffset.y, 0f);
			this.Portrait.transform.localPosition = localPosition;
			this.Portrait.transform.localScale = this.skinData.victoryPortraitScale;
		}
	}

	private void Update()
	{
	}

	private Dictionary<int, Netsuke> getNetsukeSlots(PlayerSelectionInfo selection)
	{
		Dictionary<int, Netsuke> dictionary = new Dictionary<int, Netsuke>();
		int num = PlayerUtil.FirstEquipmentSlotForType(EquipmentTypes.NETSUKE);
		int num2 = num + 3;
		if (selection.playerEquipment == null || selection.playerEquipment.Count < num2)
		{
			dictionary = this.api.GetLocalEquipmentNetsuke(selection.playerNum);
		}
		else
		{
			for (int i = num; i < num2; i++)
			{
				Netsuke netsukeFromItem = this.equipmentModel.GetNetsukeFromItem(selection.playerEquipment[i]);
				if (netsukeFromItem != null)
				{
					dictionary[i - num] = netsukeFromItem;
				}
			}
		}
		return dictionary;
	}

	private void addNetsukeView(PlayerStats stats, int totalPlayers)
	{
		Dictionary<int, Netsuke> netsukeSlots = this.getNetsukeSlots(stats.playerInfo);
		if (netsukeSlots.Count > 0)
		{
			bool flag = stats.playerInfo.playerNum == PlayerNum.Player1;
			Transform attachTo;
			if (totalPlayers > 2)
			{
				attachTo = this.NetsukeBottom;
			}
			else if (flag)
			{
				attachTo = this.NetsukeLeft;
			}
			else
			{
				attachTo = this.NetsukeRight;
			}
			this.victoryScene.AddNetsuke(netsukeSlots, attachTo, this.index, (totalPlayers > 2) ? this.netsukeBottomScale : this.netsukeLeftRightScale);
		}
	}

	private void addStat(StatType type, PlayerStats stats)
	{
		string text = this.localization.GetText("ui.victoryScreen.stat." + type);
		VictoryStatText component = UnityEngine.Object.Instantiate<GameObject>(this.StatLinePrefab).GetComponent<VictoryStatText>();
		component.transform.SetParent(this.Tray, false);
		component.StatName.text = text;
		int stat = stats.GetStat(type);
		component.StatValue.text = stats.FormatStatValue(type, stat);
	}
}

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000A82 RID: 2690
public class CharacterVictoryDisplay : MonoBehaviour
{
	// Token: 0x1700129D RID: 4765
	// (get) Token: 0x06004E9B RID: 20123 RVA: 0x00149EE6 File Offset: 0x001482E6
	// (set) Token: 0x06004E9C RID: 20124 RVA: 0x00149EEE File Offset: 0x001482EE
	[Inject]
	public GameDataManager gameData { private get; set; }

	// Token: 0x1700129E RID: 4766
	// (get) Token: 0x06004E9D RID: 20125 RVA: 0x00149EF7 File Offset: 0x001482F7
	// (set) Token: 0x06004E9E RID: 20126 RVA: 0x00149EFF File Offset: 0x001482FF
	[Inject]
	public ILocalization localization { private get; set; }

	// Token: 0x1700129F RID: 4767
	// (get) Token: 0x06004E9F RID: 20127 RVA: 0x00149F08 File Offset: 0x00148308
	// (set) Token: 0x06004EA0 RID: 20128 RVA: 0x00149F10 File Offset: 0x00148310
	[Inject]
	public IUIAdapter uiAdapter { private get; set; }

	// Token: 0x170012A0 RID: 4768
	// (get) Token: 0x06004EA1 RID: 20129 RVA: 0x00149F19 File Offset: 0x00148319
	// (set) Token: 0x06004EA2 RID: 20130 RVA: 0x00149F21 File Offset: 0x00148321
	[Inject]
	public ConfigData config { private get; set; }

	// Token: 0x170012A1 RID: 4769
	// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x00149F2A File Offset: 0x0014832A
	// (set) Token: 0x06004EA4 RID: 20132 RVA: 0x00149F32 File Offset: 0x00148332
	[Inject]
	public IVictoryScreenAPI api { private get; set; }

	// Token: 0x170012A2 RID: 4770
	// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x00149F3B File Offset: 0x0014833B
	// (set) Token: 0x06004EA6 RID: 20134 RVA: 0x00149F43 File Offset: 0x00148343
	[Inject]
	public ICharacterDataHelper characterDataHelper { private get; set; }

	// Token: 0x170012A3 RID: 4771
	// (get) Token: 0x06004EA7 RID: 20135 RVA: 0x00149F4C File Offset: 0x0014834C
	// (set) Token: 0x06004EA8 RID: 20136 RVA: 0x00149F54 File Offset: 0x00148354
	[Inject]
	public ISkinDataManager skinDataManager { private get; set; }

	// Token: 0x170012A4 RID: 4772
	// (get) Token: 0x06004EA9 RID: 20137 RVA: 0x00149F5D File Offset: 0x0014835D
	// (set) Token: 0x06004EAA RID: 20138 RVA: 0x00149F65 File Offset: 0x00148365
	[Inject]
	public IEquipmentModel equipmentModel { private get; set; }

	// Token: 0x170012A5 RID: 4773
	// (get) Token: 0x06004EAB RID: 20139 RVA: 0x00149F6E File Offset: 0x0014836E
	// (set) Token: 0x06004EAC RID: 20140 RVA: 0x00149F76 File Offset: 0x00148376
	[Inject]
	public GameDataManager gameDataManager { private get; set; }

	// Token: 0x170012A6 RID: 4774
	// (get) Token: 0x06004EAD RID: 20141 RVA: 0x00149F7F File Offset: 0x0014837F
	// (set) Token: 0x06004EAE RID: 20142 RVA: 0x00149F87 File Offset: 0x00148387
	[Inject]
	public ICharacterDataLoader characterDataLoader { private get; set; }

	// Token: 0x06004EAF RID: 20143 RVA: 0x00149F90 File Offset: 0x00148390
	public void Init(PlayerStats stats, int index, GameModeData gameModeData, SkinDefinition skin, bool isVictor, string playerName, string characterName, int totalPlayers)
	{
		this.index = index;
		this.victoryScene = this.uiAdapter.GetUIScene<VictoryScene3D>();
		UIColor uicolor = PlayerUtil.GetUIColor(stats.playerInfo, gameModeData.settings.usesTeams);
		this.PlayerNameText.text = playerName;
		this.PlayerNameText.color = this.textColors.GetColor(uicolor);
		this.skinData = this.skinDataManager.GetPreloadedSkinData(this.characterDataHelper.GetSkinDefinition(stats.playerInfo.characterID, skin.uniqueKey));
		this.Portrait.sprite = this.skinData.battlePortrait;
		this.updatePortraitPosition();
		this.PortraitBackground.sprite = this.PortraitBackgroundSprites.GetSprite(uicolor);
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

	// Token: 0x06004EB0 RID: 20144 RVA: 0x0014A0B0 File Offset: 0x001484B0
	private void updatePortraitPosition()
	{
		if (this.skinData.overrideVictoryPortraitOffset)
		{
			Vector3 localPosition = new Vector3((float)this.skinData.victoryPortraitOffset.x, (float)this.skinData.victoryPortraitOffset.y, 0f);
			this.Portrait.transform.localPosition = localPosition;
			this.Portrait.transform.localScale = this.skinData.victoryPortraitScale;
		}
	}

	// Token: 0x06004EB1 RID: 20145 RVA: 0x0014A12C File Offset: 0x0014852C
	private void Update()
	{
	}

	// Token: 0x06004EB2 RID: 20146 RVA: 0x0014A130 File Offset: 0x00148530
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

	// Token: 0x06004EB3 RID: 20147 RVA: 0x0014A1C8 File Offset: 0x001485C8
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

	// Token: 0x06004EB4 RID: 20148 RVA: 0x0014A254 File Offset: 0x00148654
	private void addStat(StatType type, PlayerStats stats)
	{
		string text = this.localization.GetText("ui.victoryScreen.stat." + type);
		VictoryStatText component = UnityEngine.Object.Instantiate<GameObject>(this.StatLinePrefab).GetComponent<VictoryStatText>();
		component.transform.SetParent(this.Tray, false);
		component.StatName.text = text;
		int stat = stats.GetStat(type);
		component.StatValue.text = stats.FormatStatValue(type, stat);
	}

	// Token: 0x04003352 RID: 13138
	public UIColorContainer textColors;

	// Token: 0x04003353 RID: 13139
	public ColorSpriteContainer PortraitBackgroundSprites;

	// Token: 0x04003354 RID: 13140
	public Image Background;

	// Token: 0x04003355 RID: 13141
	public Image PortraitBackground;

	// Token: 0x04003356 RID: 13142
	public Image Portrait;

	// Token: 0x04003357 RID: 13143
	public TextMeshProUGUI PlayerNameText;

	// Token: 0x04003358 RID: 13144
	public GameObject WinnerText;

	// Token: 0x04003359 RID: 13145
	public GameObject StatLinePrefab;

	// Token: 0x0400335A RID: 13146
	public Transform Tray;

	// Token: 0x0400335B RID: 13147
	public Sprite WinningBgSprite;

	// Token: 0x0400335C RID: 13148
	public Transform NetsukeLeft;

	// Token: 0x0400335D RID: 13149
	public Transform NetsukeRight;

	// Token: 0x0400335E RID: 13150
	public Transform NetsukeBottom;

	// Token: 0x0400335F RID: 13151
	public float netsukeLeftRightScale = 1.15f;

	// Token: 0x04003360 RID: 13152
	public float netsukeBottomScale = 0.65f;

	// Token: 0x04003361 RID: 13153
	private VictoryScene3D victoryScene;

	// Token: 0x04003362 RID: 13154
	private SkinData skinData;

	// Token: 0x04003363 RID: 13155
	private int index;
}

using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200065A RID: 1626
public class VictoryPoseController : IVictoryPoseController
{
	// Token: 0x170009C0 RID: 2496
	// (get) Token: 0x060027C3 RID: 10179 RVA: 0x000C1CBD File Offset: 0x000C00BD
	// (set) Token: 0x060027C4 RID: 10180 RVA: 0x000C1CC5 File Offset: 0x000C00C5
	[Inject]
	public ICharacterDataHelper characterDataHelper { get; set; }

	// Token: 0x170009C1 RID: 2497
	// (get) Token: 0x060027C5 RID: 10181 RVA: 0x000C1CCE File Offset: 0x000C00CE
	// (set) Token: 0x060027C6 RID: 10182 RVA: 0x000C1CD6 File Offset: 0x000C00D6
	[Inject]
	public ICharacterLists characterLists { get; set; }

	// Token: 0x170009C2 RID: 2498
	// (get) Token: 0x060027C7 RID: 10183 RVA: 0x000C1CDF File Offset: 0x000C00DF
	// (set) Token: 0x060027C8 RID: 10184 RVA: 0x000C1CE7 File Offset: 0x000C00E7
	[Inject]
	public IUserCharacterEquippedModel characterEquippedModel { get; set; }

	// Token: 0x170009C3 RID: 2499
	// (get) Token: 0x060027C9 RID: 10185 RVA: 0x000C1CF0 File Offset: 0x000C00F0
	// (set) Token: 0x060027CA RID: 10186 RVA: 0x000C1CF8 File Offset: 0x000C00F8
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170009C4 RID: 2500
	// (get) Token: 0x060027CB RID: 10187 RVA: 0x000C1D01 File Offset: 0x000C0101
	// (set) Token: 0x060027CC RID: 10188 RVA: 0x000C1D09 File Offset: 0x000C0109
	[Inject]
	public IBattleServerAPI battleServerAPI { get; set; }

	// Token: 0x170009C5 RID: 2501
	// (get) Token: 0x060027CD RID: 10189 RVA: 0x000C1D12 File Offset: 0x000C0112
	// (set) Token: 0x060027CE RID: 10190 RVA: 0x000C1D1A File Offset: 0x000C011A
	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager { get; set; }

	// Token: 0x170009C6 RID: 2502
	// (get) Token: 0x060027CF RID: 10191 RVA: 0x000C1D23 File Offset: 0x000C0123
	// (set) Token: 0x060027D0 RID: 10192 RVA: 0x000C1D2B File Offset: 0x000C012B
	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper { get; set; }

	// Token: 0x170009C7 RID: 2503
	// (get) Token: 0x060027D1 RID: 10193 RVA: 0x000C1D34 File Offset: 0x000C0134
	// (set) Token: 0x060027D2 RID: 10194 RVA: 0x000C1D3C File Offset: 0x000C013C
	[Inject]
	public IVictoryPoseHelper victoryPoseHelper { get; set; }

	// Token: 0x170009C8 RID: 2504
	// (get) Token: 0x060027D3 RID: 10195 RVA: 0x000C1D45 File Offset: 0x000C0145
	// (set) Token: 0x060027D4 RID: 10196 RVA: 0x000C1D4D File Offset: 0x000C014D
	[Inject]
	public ICharacterDataLoader characterDataLoader { get; set; }

	// Token: 0x170009C9 RID: 2505
	// (get) Token: 0x060027D5 RID: 10197 RVA: 0x000C1D56 File Offset: 0x000C0156
	// (set) Token: 0x060027D6 RID: 10198 RVA: 0x000C1D5E File Offset: 0x000C015E
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x060027D7 RID: 10199 RVA: 0x000C1D68 File Offset: 0x000C0168
	public void InitVictoryPose(VictoryScreenPayload victoryPayload, GameDataManager gameDataManager)
	{
		StageSceneData stageSceneData = UnityEngine.Object.FindObjectOfType<StageSceneData>();
		stageSceneData.GatherData();
		StageSettings stageSettings = gameDataManager.ConfigData.stageSettings;
		List<PlayerStats> sortedPlayerStats = this.GetSortedPlayerStats(victoryPayload);
		for (int i = 0; i < sortedPlayerStats.Count; i++)
		{
			PlayerStats playerStats = sortedPlayerStats[i];
			CharacterID characterID = playerStats.playerInfo.characterID;
			CharacterDefinition characterDefinition = this.characterLists.GetCharacterDefinition(characterID);
			SkinDefinition skinDefinition = this.characterDataHelper.GetSkinDefinition(characterID, playerStats.playerInfo.skinKey);
			IUISceneCharacter character = this.uiSceneCharacterManager.GetCharacter(characterID, skinDefinition);
			character.SetSkin(skinDefinition);
			character.Activate(stageSceneData.transform);
			EquippableItem equippedVictoryPoseItem = this.victoryPoseHelper.getEquippedVictoryPoseItem(playerStats.playerInfo, victoryPayload.gamePayload);
			this.previewAnimationHelper.PlayVictoryPose(character, characterDefinition, equippedVictoryPoseItem);
			Vector3 victoryPosePosition = this.GetVictoryPosePosition(stageSettings, stageSceneData, i, victoryPayload.victors.Count);
			((Component)character).transform.position = victoryPosePosition;
			((Component)character).transform.Rotate(0f, 180f, 0f);
			this.characterDisplays.Add(character);
		}
		List<AnimationClip> list = stageSettings.defaultVictoryPoseCameraAnimations;
		if (stageSceneData.SimulationData.overrideVictoryPoseCameraAnimations.Count > 0)
		{
			list = stageSceneData.SimulationData.overrideVictoryPoseCameraAnimations;
		}
		if (!gameDataManager.GameData.IsFeatureEnabled(FeatureID.PerAnimVictoryPoseCamera))
		{
			if (list.Count > 0)
			{
				int index = UnityEngine.Random.Range(0, list.Count);
				AnimationClip clip = list[index];
				foreach (Camera camera in Camera.allCameras)
				{
					if (camera.transform.parent == null || camera.transform.parent.GetComponent<Camera>() == null)
					{
						Animation animation = camera.gameObject.AddComponent<Animation>();
						animation.AddClip(clip, "victory");
						animation.Play("victory");
					}
				}
			}
			else
			{
				Debug.LogError("No victory pose camera animations set in Config -> Stage Settings");
			}
		}
		else
		{
			this.enableUpdate = true;
		}
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000C1F98 File Offset: 0x000C0398
	public void Update()
	{
		if (!this.enableUpdate)
		{
			return;
		}
		GameObject characterObject = this.characterDisplays[0].GetCharacterObject(0);
		if (characterObject == null)
		{
			Debug.LogError("Could not find primary character");
			return;
		}
		Transform transform = characterObject.transform.Find("globalMove/joints/root01/throw01");
		if (transform == null)
		{
			Debug.LogError("Could not find a throw bone for the camera animation");
			return;
		}
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000C2048 File Offset: 0x000C0448
	public void Clear()
	{
		this.enableUpdate = false;
		foreach (IUISceneCharacter characterDisplay in this.characterDisplays)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(characterDisplay);
		}
		this.characterDisplays.Clear();
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000C20BC File Offset: 0x000C04BC
	private Vector3 GetVictoryPosePosition(StageSettings stageSettings, StageSceneData stage, int index, int totalVictors)
	{
		if (totalVictors < 1)
		{
			return Vector3.zero;
		}
		Vector3ListWrapper vector3ListWrapper = null;
		if (totalVictors <= stage.SimulationData.shouldOverrideVictoryPosePositions.Count && stage.SimulationData.shouldOverrideVictoryPosePositions[totalVictors - 1])
		{
			vector3ListWrapper = stage.SimulationData.overrideVictoryPosePositions[totalVictors - 1];
		}
		if (vector3ListWrapper == null)
		{
			if (totalVictors > stageSettings.defaultVictoryPosePositions.Count)
			{
				Debug.Log("No default settings in config for victory pose positions for (" + totalVictors + ") winners");
				return Vector3.zero;
			}
			vector3ListWrapper = stageSettings.defaultVictoryPosePositions[totalVictors - 1];
		}
		return vector3ListWrapper.list[index];
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x000C2178 File Offset: 0x000C0578
	private List<PlayerStats> GetSortedPlayerStats(VictoryScreenPayload victoryPayload)
	{
		List<PlayerStats> list = new List<PlayerStats>();
		if (victoryPayload.victors.Count > 0)
		{
			foreach (PlayerNum playerNum in victoryPayload.victors)
			{
				foreach (PlayerStats playerStats in victoryPayload.stats)
				{
					if (playerStats.playerInfo.playerNum == playerNum)
					{
						list.Add(playerStats);
						break;
					}
				}
			}
		}
		else
		{
			bool flag = false;
			TeamNum[] values = EnumUtil.GetValues<TeamNum>();
			foreach (TeamNum teamNum in values)
			{
				list.Clear();
				if (PlayerUtil.IsValidTeam(teamNum))
				{
					foreach (PlayerStats playerStats2 in victoryPayload.stats)
					{
						if (playerStats2.playerInfo.team == teamNum && playerStats2.playerInfo.type != PlayerType.None && !playerStats2.playerInfo.isSpectator && PlayerUtil.IsValidPlayer(playerStats2.playerInfo.playerNum))
						{
							list.Add(playerStats2);
							if (!victoryPayload.gamePayload.isOnlineGame || this.battleServerAPI.LocalPlayerNumIDs.Contains(playerStats2.playerInfo.playerNum))
							{
								flag = true;
							}
						}
					}
					if (flag)
					{
						break;
					}
				}
			}
		}
		list.Sort((PlayerStats a, PlayerStats b) => b.GetStat(StatType.Kill) - a.GetStat(StatType.Kill));
		return list;
	}

	// Token: 0x04001D1D RID: 7453
	private bool enableUpdate;

	// Token: 0x04001D1E RID: 7454
	private List<IUISceneCharacter> characterDisplays = new List<IUISceneCharacter>();
}

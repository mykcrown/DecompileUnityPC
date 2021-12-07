// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class VictoryPoseController : IVictoryPoseController
{
	private bool enableUpdate;

	private List<IUISceneCharacter> characterDisplays = new List<IUISceneCharacter>();

	private static Comparison<PlayerStats> __f__am_cache0;

	[Inject]
	public ICharacterDataHelper characterDataHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterLists characterLists
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel characterEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IBattleServerAPI battleServerAPI
	{
		get;
		set;
	}

	[Inject]
	public IUISceneCharacterManager uiSceneCharacterManager
	{
		get;
		set;
	}

	[Inject]
	public IPreviewAnimationHelper previewAnimationHelper
	{
		get;
		set;
	}

	[Inject]
	public IVictoryPoseHelper victoryPoseHelper
	{
		get;
		set;
	}

	[Inject]
	public ICharacterDataLoader characterDataLoader
	{
		get;
		set;
	}

	[Inject]
	public ConfigData config
	{
		get;
		set;
	}

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
				Camera[] allCameras = Camera.allCameras;
				for (int j = 0; j < allCameras.Length; j++)
				{
					Camera camera = allCameras[j];
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
				UnityEngine.Debug.LogError("No victory pose camera animations set in Config -> Stage Settings");
			}
		}
		else
		{
			this.enableUpdate = true;
		}
	}

	public void Update()
	{
		if (!this.enableUpdate)
		{
			return;
		}
		GameObject characterObject = this.characterDisplays[0].GetCharacterObject(0);
		if (characterObject == null)
		{
			UnityEngine.Debug.LogError("Could not find primary character");
			return;
		}
		Transform transform = characterObject.transform.Find("globalMove/joints/root01/throw01");
		if (transform == null)
		{
			UnityEngine.Debug.LogError("Could not find a throw bone for the camera animation");
			return;
		}
		Camera.main.transform.position = transform.position;
		Camera.main.transform.rotation = transform.rotation * Quaternion.Euler(0f, 180f, 0f);
	}

	public void Clear()
	{
		this.enableUpdate = false;
		foreach (IUISceneCharacter current in this.characterDisplays)
		{
			this.uiSceneCharacterManager.ReleaseCharacter(current);
		}
		this.characterDisplays.Clear();
	}

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
				UnityEngine.Debug.Log("No default settings in config for victory pose positions for (" + totalVictors + ") winners");
				return Vector3.zero;
			}
			vector3ListWrapper = stageSettings.defaultVictoryPosePositions[totalVictors - 1];
		}
		return vector3ListWrapper.list[index];
	}

	private List<PlayerStats> GetSortedPlayerStats(VictoryScreenPayload victoryPayload)
	{
		List<PlayerStats> list = new List<PlayerStats>();
		if (victoryPayload.victors.Count > 0)
		{
			foreach (PlayerNum current in victoryPayload.victors)
			{
				foreach (PlayerStats current2 in victoryPayload.stats)
				{
					if (current2.playerInfo.playerNum == current)
					{
						list.Add(current2);
						break;
					}
				}
			}
		}
		else
		{
			bool flag = false;
			TeamNum[] values = EnumUtil.GetValues<TeamNum>();
			TeamNum[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				TeamNum teamNum = array[i];
				list.Clear();
				if (PlayerUtil.IsValidTeam(teamNum))
				{
					foreach (PlayerStats current3 in victoryPayload.stats)
					{
						if (current3.playerInfo.team == teamNum && current3.playerInfo.type != PlayerType.None && !current3.playerInfo.isSpectator && PlayerUtil.IsValidPlayer(current3.playerInfo.playerNum))
						{
							list.Add(current3);
							if (!victoryPayload.gamePayload.isOnlineGame || this.battleServerAPI.LocalPlayerNumIDs.Contains(current3.playerInfo.playerNum))
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
		List<PlayerStats> arg_1DB_0 = list;
		if (VictoryPoseController.__f__am_cache0 == null)
		{
			VictoryPoseController.__f__am_cache0 = new Comparison<PlayerStats>(VictoryPoseController._GetSortedPlayerStats_m__0);
		}
		arg_1DB_0.Sort(VictoryPoseController.__f__am_cache0);
		return list;
	}

	private static int _GetSortedPlayerStats_m__0(PlayerStats a, PlayerStats b)
	{
		return b.GetStat(StatType.Kill) - a.GetStat(StatType.Kill);
	}
}

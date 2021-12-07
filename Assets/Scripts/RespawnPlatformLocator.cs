// Decompile from assembly: Assembly-CSharp.dll

using System;
using UnityEngine;

public class RespawnPlatformLocator : IRespawnPlatformLocator
{
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
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
	public GameDataManager gameDataManager
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
	public IItemLoader itemLoader
	{
		get;
		set;
	}

	[Inject]
	public IUserInputManager userInputManager
	{
		get;
		set;
	}

	public GameObject GetPrefab(PlayerSelectionInfo playerInfo)
	{
		UnityEngine.Debug.Log("Load custom platform prefab");
		EquippableItem customItem = this.GetCustomItem(playerInfo);
		if (customItem != null)
		{
			UnityEngine.Debug.Log(string.Concat(new object[]
			{
				"Try loading for item ",
				customItem.id,
				" ",
				customItem.backupNameText
			}));
			try
			{
				CustomPlatform customPlatform = this.itemLoader.LoadPrefab<CustomPlatform>(customItem);
				if (!(customPlatform == null))
				{
					return customPlatform.gameObject;
				}
				UnityEngine.Debug.LogError("Custom platform with local asset id " + customItem.localAssetId + " not found: ");
			}
			catch (UnityException ex)
			{
				UnityEngine.Debug.LogError("ERROR loading respawn platform " + customItem.backupNameText + " - " + ex.Message);
			}
		}
		return null;
	}

	public EquippableItem GetCustomItem(PlayerSelectionInfo playerInfo)
	{
		int bestPortId = this.userInputManager.GetBestPortId(playerInfo.playerNum);
		bool flag = this.enterNewGame.GamePayload != null && this.enterNewGame.GamePayload.isOnlineGame;
		EquipmentID id = (!flag) ? this.userCharacterEquippedModel.GetEquippedByType(EquipmentTypes.PLATFORM, playerInfo.characterID, bestPortId) : playerInfo.characterEquipment[PlayerUtil.FirstEquipmentSlotForType(EquipmentTypes.PLATFORM)];
		if (!id.IsNull())
		{
			EquippableItem item = this.equipmentModel.GetItem(id);
			if (item != null)
			{
				if (item.localAssetId != null)
				{
					return item;
				}
			}
		}
		return null;
	}
}

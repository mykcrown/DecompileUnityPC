using System;
using UnityEngine;

// Token: 0x02000613 RID: 1555
public class RespawnPlatformLocator : IRespawnPlatformLocator
{
	// Token: 0x17000972 RID: 2418
	// (get) Token: 0x06002653 RID: 9811 RVA: 0x000BC9F2 File Offset: 0x000BADF2
	// (set) Token: 0x06002654 RID: 9812 RVA: 0x000BC9FA File Offset: 0x000BADFA
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17000973 RID: 2419
	// (get) Token: 0x06002655 RID: 9813 RVA: 0x000BCA03 File Offset: 0x000BAE03
	// (set) Token: 0x06002656 RID: 9814 RVA: 0x000BCA0B File Offset: 0x000BAE0B
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000974 RID: 2420
	// (get) Token: 0x06002657 RID: 9815 RVA: 0x000BCA14 File Offset: 0x000BAE14
	// (set) Token: 0x06002658 RID: 9816 RVA: 0x000BCA1C File Offset: 0x000BAE1C
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000975 RID: 2421
	// (get) Token: 0x06002659 RID: 9817 RVA: 0x000BCA25 File Offset: 0x000BAE25
	// (set) Token: 0x0600265A RID: 9818 RVA: 0x000BCA2D File Offset: 0x000BAE2D
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x17000976 RID: 2422
	// (get) Token: 0x0600265B RID: 9819 RVA: 0x000BCA36 File Offset: 0x000BAE36
	// (set) Token: 0x0600265C RID: 9820 RVA: 0x000BCA3E File Offset: 0x000BAE3E
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x17000977 RID: 2423
	// (get) Token: 0x0600265D RID: 9821 RVA: 0x000BCA47 File Offset: 0x000BAE47
	// (set) Token: 0x0600265E RID: 9822 RVA: 0x000BCA4F File Offset: 0x000BAE4F
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x0600265F RID: 9823 RVA: 0x000BCA58 File Offset: 0x000BAE58
	public GameObject GetPrefab(PlayerSelectionInfo playerInfo)
	{
		Debug.Log("Load custom platform prefab");
		EquippableItem customItem = this.GetCustomItem(playerInfo);
		if (customItem != null)
		{
			Debug.Log(string.Concat(new object[]
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
				Debug.LogError("Custom platform with local asset id " + customItem.localAssetId + " not found: ");
			}
			catch (UnityException ex)
			{
				Debug.LogError("ERROR loading respawn platform " + customItem.backupNameText + " - " + ex.Message);
			}
		}
		return null;
	}

	// Token: 0x06002660 RID: 9824 RVA: 0x000BCB38 File Offset: 0x000BAF38
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

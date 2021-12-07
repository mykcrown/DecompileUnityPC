using System;
using System.Collections.Generic;
using System.Linq;

// Token: 0x0200048E RID: 1166
public class GameItemsPreloader : IGameItemsPreloader
{
	// Token: 0x17000532 RID: 1330
	// (get) Token: 0x06001949 RID: 6473 RVA: 0x000842AB File Offset: 0x000826AB
	// (set) Token: 0x0600194A RID: 6474 RVA: 0x000842B3 File Offset: 0x000826B3
	[Inject]
	public IRespawnPlatformLocator respawnPlatformLocator { get; set; }

	// Token: 0x17000533 RID: 1331
	// (get) Token: 0x0600194B RID: 6475 RVA: 0x000842BC File Offset: 0x000826BC
	// (set) Token: 0x0600194C RID: 6476 RVA: 0x000842C4 File Offset: 0x000826C4
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17000534 RID: 1332
	// (get) Token: 0x0600194D RID: 6477 RVA: 0x000842CD File Offset: 0x000826CD
	// (set) Token: 0x0600194E RID: 6478 RVA: 0x000842D5 File Offset: 0x000826D5
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x17000535 RID: 1333
	// (get) Token: 0x0600194F RID: 6479 RVA: 0x000842DE File Offset: 0x000826DE
	// (set) Token: 0x06001950 RID: 6480 RVA: 0x000842E6 File Offset: 0x000826E6
	[Inject]
	public IPlayerTauntsFinder playerTaunts { get; set; }

	// Token: 0x17000536 RID: 1334
	// (get) Token: 0x06001951 RID: 6481 RVA: 0x000842EF File Offset: 0x000826EF
	// (set) Token: 0x06001952 RID: 6482 RVA: 0x000842F7 File Offset: 0x000826F7
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000537 RID: 1335
	// (get) Token: 0x06001953 RID: 6483 RVA: 0x00084300 File Offset: 0x00082700
	// (set) Token: 0x06001954 RID: 6484 RVA: 0x00084308 File Offset: 0x00082708
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000538 RID: 1336
	// (get) Token: 0x06001955 RID: 6485 RVA: 0x00084311 File Offset: 0x00082711
	// (set) Token: 0x06001956 RID: 6486 RVA: 0x00084319 File Offset: 0x00082719
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x06001957 RID: 6487 RVA: 0x00084322 File Offset: 0x00082722
	public void Preload(GameLoadPayload payload, Action callback)
	{
		this.recursiveLoadPlayers(payload.players, 0, callback);
	}

	// Token: 0x06001958 RID: 6488 RVA: 0x00084334 File Offset: 0x00082734
	private void recursiveLoadPlayers(PlayerSelectionList list, int i, Action callback)
	{
		if (i > list.Length - 1)
		{
			callback();
		}
		else
		{
			PlayerSelectionInfo info = list[i];
			if (info.type == PlayerType.None || info.isSpectator)
			{
				this.recursiveLoadPlayers(list, i + 1, callback);
			}
			else
			{
				this.loadPlayerPlatform(info, delegate
				{
					this.loadPlayerVictoryPose(info, delegate
					{
						this.loadPlayerEmotes(info, delegate
						{
							this.recursiveLoadPlayers(list, i + 1, callback);
						});
					});
				});
			}
		}
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x00084408 File Offset: 0x00082808
	private void loadPlayerPlatform(PlayerSelectionInfo info, Action callback)
	{
		EquippableItem customItem = this.respawnPlatformLocator.GetCustomItem(info);
		if (customItem == null)
		{
			callback();
		}
		else
		{
			this.itemLoader.Preload<CustomPlatform>(customItem, callback);
		}
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x00084440 File Offset: 0x00082840
	private void loadPlayerVictoryPose(PlayerSelectionInfo info, Action callback)
	{
		int bestPortId = this.userInputManager.GetBestPortId(info.playerNum);
		EquippableItem equippedItem = this.userCharacterEquippedModel.GetEquippedItem(EquipmentTypes.VICTORY_POSE, info.characterID, bestPortId);
		if (equippedItem == null)
		{
			callback();
		}
		else
		{
			this.itemLoader.PreloadAsset(equippedItem, callback);
		}
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x00084494 File Offset: 0x00082894
	private void loadPlayerEmotes(PlayerSelectionInfo info, Action callback)
	{
		UserTaunts forPlayer = this.playerTaunts.GetForPlayer(info.playerNum);
		List<EquippableItem> loadList = null;
		if (forPlayer != null)
		{
			loadList = (from kv in forPlayer.GetSlotsForCharacter(info.characterID)
			select this.equipmentModel.GetItem(kv.Value) into item
			where item != null && item.type == EquipmentTypes.EMOTE
			select item).ToList<EquippableItem>();
		}
		this.preloadEmotes(loadList, callback);
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x00084508 File Offset: 0x00082908
	private void preloadEmotes(List<EquippableItem> loadList, Action callback)
	{
		if (loadList == null || loadList.Count == 0)
		{
			callback();
		}
		else
		{
			ReferenceValue<int> remaining = new ReferenceValue<int>(loadList.Count);
			foreach (EquippableItem item in loadList)
			{
				this.itemLoader.PreloadAsset(item, delegate
				{
					remaining.Value--;
					if (remaining.Value == 0)
					{
						callback();
					}
				});
			}
		}
	}
}

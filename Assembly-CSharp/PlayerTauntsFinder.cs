using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200060D RID: 1549
public class PlayerTauntsFinder : IPlayerTauntsFinder
{
	// Token: 0x17000969 RID: 2409
	// (get) Token: 0x06002627 RID: 9767 RVA: 0x000BC308 File Offset: 0x000BA708
	// (set) Token: 0x06002628 RID: 9768 RVA: 0x000BC310 File Offset: 0x000BA710
	[Inject]
	public IUserTauntsModel userTauntsModel { get; set; }

	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06002629 RID: 9769 RVA: 0x000BC319 File Offset: 0x000BA719
	// (set) Token: 0x0600262A RID: 9770 RVA: 0x000BC321 File Offset: 0x000BA721
	[Inject]
	public IEnterNewGame enterNewGame { get; set; }

	// Token: 0x1700096B RID: 2411
	// (get) Token: 0x0600262B RID: 9771 RVA: 0x000BC32A File Offset: 0x000BA72A
	// (set) Token: 0x0600262C RID: 9772 RVA: 0x000BC332 File Offset: 0x000BA732
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700096C RID: 2412
	// (get) Token: 0x0600262D RID: 9773 RVA: 0x000BC33B File Offset: 0x000BA73B
	// (set) Token: 0x0600262E RID: 9774 RVA: 0x000BC343 File Offset: 0x000BA743
	[Inject]
	public IItemLoader itemLoader { get; set; }

	// Token: 0x1700096D RID: 2413
	// (get) Token: 0x0600262F RID: 9775 RVA: 0x000BC34C File Offset: 0x000BA74C
	// (set) Token: 0x06002630 RID: 9776 RVA: 0x000BC354 File Offset: 0x000BA754
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x1700096E RID: 2414
	// (get) Token: 0x06002631 RID: 9777 RVA: 0x000BC35D File Offset: 0x000BA75D
	// (set) Token: 0x06002632 RID: 9778 RVA: 0x000BC365 File Offset: 0x000BA765
	[Inject]
	public IReplaySystem replaySystem { get; set; }

	// Token: 0x06002633 RID: 9779 RVA: 0x000BC370 File Offset: 0x000BA770
	public UserTaunts GetForPlayer(PlayerNum player)
	{
		if (this.enterNewGame.GamePayload != null && (this.enterNewGame.GamePayload.isOnlineGame || this.replaySystem.IsReplaying))
		{
			return this.enterNewGame.GamePayload.FindPlayerInfo(player).tauntData;
		}
		return this.userTauntsModel.GetDataObject(this.userInputManager.GetBestPortId(player));
	}

	// Token: 0x06002634 RID: 9780 RVA: 0x000BC3E0 File Offset: 0x000BA7E0
	public List<MoveData> GetPlayerEmoteMoveData(PlayerNum playerNum, CharacterID characterID)
	{
		List<MoveData> list = new List<MoveData>();
		if (characterID != CharacterID.None)
		{
			UserTaunts forPlayer = this.GetForPlayer(playerNum);
			if (forPlayer == null)
			{
				throw new UnityException("Didn't find taunts for player " + playerNum);
			}
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.LEFT), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.RIGHT), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.UP), list);
			this.AddEmoteMoveFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.DOWN), list);
		}
		return list;
	}

	// Token: 0x06002635 RID: 9781 RVA: 0x000BC45C File Offset: 0x000BA85C
	public List<HologramData> GetPlayerHologramData(PlayerNum playerNum, CharacterID characterID)
	{
		List<HologramData> list = new List<HologramData>();
		if (characterID != CharacterID.None)
		{
			UserTaunts forPlayer = this.GetForPlayer(playerNum);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.LEFT), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.RIGHT), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.UP), list);
			this.AddHologramDataFromEquipmentID(forPlayer.GetItemInSlot(characterID, TauntSlot.DOWN), list);
		}
		return list;
	}

	// Token: 0x06002636 RID: 9782 RVA: 0x000BC4BC File Offset: 0x000BA8BC
	private void AddEmoteMoveFromEquipmentID(EquipmentID equipmentID, List<MoveData> emoteMoves)
	{
		EquippableItem item = this.equipmentModel.GetItem(equipmentID);
		if (item != null && item.type == EquipmentTypes.EMOTE)
		{
			EmoteData emoteData = this.itemLoader.LoadAsset<EmoteData>(item);
			if (emoteData != null)
			{
				if (emoteData.primaryData != null)
				{
					emoteMoves.Add(emoteData.primaryData);
				}
				if (emoteData.partnerData != null)
				{
					emoteMoves.Add(emoteData.partnerData);
				}
			}
		}
	}

	// Token: 0x06002637 RID: 9783 RVA: 0x000BC53C File Offset: 0x000BA93C
	private void AddHologramDataFromEquipmentID(EquipmentID equipmentID, List<HologramData> holograms)
	{
		EquippableItem item = this.equipmentModel.GetItem(equipmentID);
		if (item != null && item.type == EquipmentTypes.HOLOGRAM)
		{
			HologramData hologramData = this.itemLoader.LoadAsset<HologramData>(item);
			if (hologramData != null)
			{
				holograms.Add(hologramData);
			}
		}
	}
}

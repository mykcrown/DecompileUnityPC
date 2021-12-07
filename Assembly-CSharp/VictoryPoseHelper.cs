using System;

// Token: 0x0200065C RID: 1628
public class VictoryPoseHelper : IVictoryPoseHelper
{
	// Token: 0x170009CA RID: 2506
	// (get) Token: 0x060027E1 RID: 10209 RVA: 0x000C23A9 File Offset: 0x000C07A9
	// (set) Token: 0x060027E2 RID: 10210 RVA: 0x000C23B1 File Offset: 0x000C07B1
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x170009CB RID: 2507
	// (get) Token: 0x060027E3 RID: 10211 RVA: 0x000C23BA File Offset: 0x000C07BA
	// (set) Token: 0x060027E4 RID: 10212 RVA: 0x000C23C2 File Offset: 0x000C07C2
	[Inject]
	public IUserCharacterEquippedModel userEquipmentModel { get; set; }

	// Token: 0x170009CC RID: 2508
	// (get) Token: 0x060027E5 RID: 10213 RVA: 0x000C23CB File Offset: 0x000C07CB
	// (set) Token: 0x060027E6 RID: 10214 RVA: 0x000C23D3 File Offset: 0x000C07D3
	[Inject]
	public IUserInputManager userInputManager { get; set; }

	// Token: 0x060027E7 RID: 10215 RVA: 0x000C23DC File Offset: 0x000C07DC
	public EquippableItem getEquippedVictoryPoseItem(PlayerSelectionInfo playerInfo, GameLoadPayload gamePayload)
	{
		EquippableItem equippableItem;
		if (gamePayload != null && gamePayload.isOnlineGame)
		{
			equippableItem = this.equipmentModel.GetItem(playerInfo.characterEquipment[PlayerUtil.FirstEquipmentSlotForType(EquipmentTypes.VICTORY_POSE)]);
			if (equippableItem == null)
			{
				equippableItem = this.equipmentModel.GetDefaultItem(playerInfo.characterID, EquipmentTypes.VICTORY_POSE);
			}
		}
		else
		{
			int bestPortId = this.userInputManager.GetBestPortId(playerInfo.playerNum);
			equippableItem = this.userEquipmentModel.GetEquippedItem(EquipmentTypes.VICTORY_POSE, playerInfo.characterID, bestPortId);
		}
		return equippableItem;
	}
}

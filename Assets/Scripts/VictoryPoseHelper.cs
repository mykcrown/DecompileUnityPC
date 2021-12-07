// Decompile from assembly: Assembly-CSharp.dll

using System;

public class VictoryPoseHelper : IVictoryPoseHelper
{
	[Inject]
	public IEquipmentModel equipmentModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userEquipmentModel
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

using System;
using System.Collections.Generic;

// Token: 0x02000748 RID: 1864
public class UserInventoryModel : IUserInventory
{
	// Token: 0x17000B4A RID: 2890
	// (get) Token: 0x06002E2C RID: 11820 RVA: 0x000EA839 File Offset: 0x000E8C39
	// (set) Token: 0x06002E2D RID: 11821 RVA: 0x000EA841 File Offset: 0x000E8C41
	[Inject]
	public GameDataManager gameDataManager { get; set; }

	// Token: 0x17000B4B RID: 2891
	// (get) Token: 0x06002E2E RID: 11822 RVA: 0x000EA84A File Offset: 0x000E8C4A
	// (set) Token: 0x06002E2F RID: 11823 RVA: 0x000EA852 File Offset: 0x000E8C52
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17000B4C RID: 2892
	// (get) Token: 0x06002E30 RID: 11824 RVA: 0x000EA85B File Offset: 0x000E8C5B
	// (set) Token: 0x06002E31 RID: 11825 RVA: 0x000EA863 File Offset: 0x000E8C63
	[Inject]
	public IInventorySource source { get; set; }

	// Token: 0x17000B4D RID: 2893
	// (get) Token: 0x06002E32 RID: 11826 RVA: 0x000EA86C File Offset: 0x000E8C6C
	// (set) Token: 0x06002E33 RID: 11827 RVA: 0x000EA874 File Offset: 0x000E8C74
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x06002E34 RID: 11828 RVA: 0x000EA87D File Offset: 0x000E8C7D
	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserInventoryModel.SOURCE_UPDATED, new Action(this.onInventoryUpdated));
		this.onInventoryUpdated();
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x000EA8A4 File Offset: 0x000E8CA4
	private void onInventoryUpdated()
	{
		foreach (EquipmentID itemId in this.source.GetOwned())
		{
			this.AddItem(itemId, false);
			this.MarkAsNotNew(itemId, true);
		}
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x000EA8F0 File Offset: 0x000E8CF0
	public void AddItem(EquipmentID itemId, bool dispatchUpdate = true)
	{
		EquippableItem item = this.equipmentModel.GetItem(itemId);
		if (item != null)
		{
			bool flag = false;
			if (item.unique)
			{
				if (!this.equipmentInventory.ContainsKey(itemId) || this.equipmentInventory[itemId] != 1)
				{
					flag |= true;
					this.equipmentInventory[itemId] = 1;
				}
			}
			else if (!this.equipmentInventory.ContainsKey(itemId))
			{
				this.equipmentInventory[itemId] = 1;
			}
			else
			{
				Dictionary<EquipmentID, int> dictionary;
				(dictionary = this.equipmentInventory)[itemId] = dictionary[itemId] + 1;
			}
			if (flag && !item.isDefault)
			{
				if (item.character == CharacterID.None)
				{
					this.markedAsNewGlobal[item.type] = true;
				}
				else
				{
					this.markedAsNewCharacter[item.character] = true;
				}
				this.markedAsNewEquipment[itemId] = true;
				this.markedAsNewAny = true;
			}
			if (flag && dispatchUpdate)
			{
				this.signalBus.Dispatch(UserInventoryModel.UPDATED);
			}
		}
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x000EAA08 File Offset: 0x000E8E08
	public bool HasItem(EquipmentID itemId)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
		{
			return true;
		}
		EquippableItem item = this.equipmentModel.GetItem(itemId);
		return item.isDefault || (this.equipmentInventory.ContainsKey(itemId) && this.equipmentInventory[itemId] > 0);
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x000EAA68 File Offset: 0x000E8E68
	public bool HasSkin(SkinDefinition skin)
	{
		if (skin == null)
		{
			return true;
		}
		EquippableItem itemFromSkinKey = this.equipmentModel.GetItemFromSkinKey(skin.uniqueKey);
		if (this.gameDataManager.GameData.IsFeatureEnabled(FeatureID.UnlockEverything))
		{
			return skin.isDefault || itemFromSkinKey == null || this.HasItem(itemFromSkinKey.id);
		}
		return skin.isDefault || (itemFromSkinKey != null && this.HasItem(itemFromSkinKey.id));
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x000EAAEF File Offset: 0x000E8EEF
	public bool IsNew(EquipmentID itemId)
	{
		return this.HasItem(itemId) && this.markedAsNewEquipment.ContainsKey(itemId);
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x000EAB0B File Offset: 0x000E8F0B
	public bool HasAnyNewItem()
	{
		return this.markedAsNewAny;
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x000EAB13 File Offset: 0x000E8F13
	public bool HasNewCharacterItem(CharacterID characterID)
	{
		return this.markedAsNewCharacter.ContainsKey(characterID);
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x000EAB21 File Offset: 0x000E8F21
	public bool HasNewGlobalItem(EquipmentTypes type)
	{
		return this.markedAsNewGlobal.ContainsKey(type);
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x000EAB30 File Offset: 0x000E8F30
	public void MarkAsNotNew(EquipmentID itemId, bool dispatchUpdate = true)
	{
		if (this.markedAsNewEquipment.ContainsKey(itemId))
		{
			EquippableItem item = this.equipmentModel.GetItem(itemId);
			this.markedAsNewEquipment.Remove(itemId);
			this.markedAsNewCharacter.Remove(item.character);
			this.markedAsNewGlobal.Remove(item.type);
			this.markedAsNewAny = false;
			if (dispatchUpdate)
			{
				this.signalBus.Dispatch(UserInventoryModel.UPDATED);
			}
		}
	}

	// Token: 0x06002E3E RID: 11838 RVA: 0x000EABA9 File Offset: 0x000E8FA9
	public void MarkAsNotNewCharacter(CharacterID characterId, bool dispatchUpdate = true)
	{
		if (this.markedAsNewCharacter.ContainsKey(characterId))
		{
			this.markedAsNewCharacter.Remove(characterId);
			this.markedAsNewAny = false;
			if (dispatchUpdate)
			{
				this.signalBus.Dispatch(UserInventoryModel.UPDATED);
			}
		}
	}

	// Token: 0x06002E3F RID: 11839 RVA: 0x000EABE6 File Offset: 0x000E8FE6
	public void MarkAsNotNewGlobal(EquipmentTypes type, bool dispatchUpdate = true)
	{
		if (this.markedAsNewGlobal.ContainsKey(type))
		{
			this.markedAsNewGlobal.Remove(type);
			this.markedAsNewAny = false;
			if (dispatchUpdate)
			{
				this.signalBus.Dispatch(UserInventoryModel.UPDATED);
			}
		}
	}

	// Token: 0x06002E40 RID: 11840 RVA: 0x000EAC23 File Offset: 0x000E9023
	public void MarkAsNotNewAny(bool dispatchUpdate = true)
	{
		if (this.markedAsNewAny)
		{
			this.markedAsNewAny = false;
			if (dispatchUpdate)
			{
				this.signalBus.Dispatch(UserInventoryModel.UPDATED);
			}
		}
	}

	// Token: 0x06002E41 RID: 11841 RVA: 0x000EAC50 File Offset: 0x000E9050
	public int GetAllOwnedCharacterItemCount(CharacterID characterID)
	{
		EquippableItem[] allCharacterItems = this.equipmentModel.GetAllCharacterItems(characterID);
		int num = 0;
		foreach (EquippableItem equippableItem in allCharacterItems)
		{
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002E42 RID: 11842 RVA: 0x000EACA4 File Offset: 0x000E90A4
	public int GetOwnedCharacterItemCount(CharacterID characterID, EquipmentTypes type)
	{
		EquippableItem[] characterItems = this.equipmentModel.GetCharacterItems(characterID, type);
		int num = 0;
		foreach (EquippableItem equippableItem in characterItems)
		{
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06002E43 RID: 11843 RVA: 0x000EACF8 File Offset: 0x000E90F8
	public int GetOwnedGlobalItemCount(EquipmentTypes type)
	{
		EquippableItem[] globalItems = this.equipmentModel.GetGlobalItems(type);
		int num = 0;
		foreach (EquippableItem equippableItem in globalItems)
		{
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x04002087 RID: 8327
	public static string UPDATED = "UserInventoryModel.UPDATE";

	// Token: 0x04002088 RID: 8328
	public static string SOURCE_UPDATED = "UserInventoryModel.SOURCE_UPDATED";

	// Token: 0x0400208D RID: 8333
	private Dictionary<EquipmentID, int> equipmentInventory = new Dictionary<EquipmentID, int>();

	// Token: 0x0400208E RID: 8334
	private Dictionary<EquipmentID, bool> markedAsNewEquipment = new Dictionary<EquipmentID, bool>();

	// Token: 0x0400208F RID: 8335
	private Dictionary<CharacterID, bool> markedAsNewCharacter = new Dictionary<CharacterID, bool>();

	// Token: 0x04002090 RID: 8336
	private Dictionary<EquipmentTypes, bool> markedAsNewGlobal = new Dictionary<EquipmentTypes, bool>();

	// Token: 0x04002091 RID: 8337
	private bool markedAsNewAny;
}

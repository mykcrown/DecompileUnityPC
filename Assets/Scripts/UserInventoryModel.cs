// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class UserInventoryModel : IUserInventory
{
	public static string UPDATED = "UserInventoryModel.UPDATE";

	public static string SOURCE_UPDATED = "UserInventoryModel.SOURCE_UPDATED";

	private Dictionary<EquipmentID, int> equipmentInventory = new Dictionary<EquipmentID, int>();

	private Dictionary<EquipmentID, bool> markedAsNewEquipment = new Dictionary<EquipmentID, bool>();

	private Dictionary<CharacterID, bool> markedAsNewCharacter = new Dictionary<CharacterID, bool>();

	private Dictionary<EquipmentTypes, bool> markedAsNewGlobal = new Dictionary<EquipmentTypes, bool>();

	private bool markedAsNewAny;

	[Inject]
	public GameDataManager gameDataManager
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
	public IInventorySource source
	{
		get;
		set;
	}

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[PostConstruct]
	public void Init()
	{
		this.signalBus.AddListener(UserInventoryModel.SOURCE_UPDATED, new Action(this.onInventoryUpdated));
		this.onInventoryUpdated();
	}

	private void onInventoryUpdated()
	{
		EquipmentID[] owned = this.source.GetOwned();
		for (int i = 0; i < owned.Length; i++)
		{
			EquipmentID itemId = owned[i];
			this.AddItem(itemId, false);
			this.MarkAsNotNew(itemId, true);
		}
	}

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

	public bool HasItem(EquipmentID itemId)
	{
		if (this.gameDataManager.IsFeatureEnabled(FeatureID.UnlockEverythingOnline))
		{
			return true;
		}
		EquippableItem item = this.equipmentModel.GetItem(itemId);
		return item.isDefault || (this.equipmentInventory.ContainsKey(itemId) && this.equipmentInventory[itemId] > 0);
	}

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

	public bool IsNew(EquipmentID itemId)
	{
		return this.HasItem(itemId) && this.markedAsNewEquipment.ContainsKey(itemId);
	}

	public bool HasAnyNewItem()
	{
		return this.markedAsNewAny;
	}

	public bool HasNewCharacterItem(CharacterID characterID)
	{
		return this.markedAsNewCharacter.ContainsKey(characterID);
	}

	public bool HasNewGlobalItem(EquipmentTypes type)
	{
		return this.markedAsNewGlobal.ContainsKey(type);
	}

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

	public int GetAllOwnedCharacterItemCount(CharacterID characterID)
	{
		EquippableItem[] allCharacterItems = this.equipmentModel.GetAllCharacterItems(characterID);
		int num = 0;
		EquippableItem[] array = allCharacterItems;
		for (int i = 0; i < array.Length; i++)
		{
			EquippableItem equippableItem = array[i];
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}

	public int GetOwnedCharacterItemCount(CharacterID characterID, EquipmentTypes type)
	{
		EquippableItem[] characterItems = this.equipmentModel.GetCharacterItems(characterID, type);
		int num = 0;
		EquippableItem[] array = characterItems;
		for (int i = 0; i < array.Length; i++)
		{
			EquippableItem equippableItem = array[i];
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}

	public int GetOwnedGlobalItemCount(EquipmentTypes type)
	{
		EquippableItem[] globalItems = this.equipmentModel.GetGlobalItems(type);
		int num = 0;
		EquippableItem[] array = globalItems;
		for (int i = 0; i < array.Length; i++)
		{
			EquippableItem equippableItem = array[i];
			if (this.HasItem(equippableItem.id))
			{
				num++;
			}
		}
		return num;
	}
}

// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSelectorAPI : IEquipModuleAPI
{
	public static string UPDATED = "EquipmentSelectorAPI.UPDATED";

	private Dictionary<EquipmentTypes, EquipTypeDefinition> equipTypeDefIndex;

	private EquipTypeDefinition[] equipmentTypes;

	private EquipmentTypes[] validTypes;

	private EquipTypeDefinition[] validEquipDefs;

	private EquipmentTypes selectedEquipType;

	private EquippableItem selectedEquipment;

	private EquipmentSelectorModule.MenuType selectedMenuType;

	[Inject]
	public ISignalBus signalBus
	{
		get;
		set;
	}

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	[Inject]
	public IUserInventory userInventory
	{
		get;
		set;
	}

	[Inject]
	public IEquipMethodMap equipMethodMap
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel
	{
		get;
		set;
	}

	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel
	{
		get;
		set;
	}

	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel
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
	public IUserTauntsModel userTauntsModel
	{
		get;
		set;
	}

	[Inject]
	public ICharacterEquipViewAPI characterEquipViewAPI
	{
		get;
		set;
	}

	public EquipmentSelectorModule.MenuType SelectedMenuType
	{
		get
		{
			return this.selectedMenuType;
		}
		set
		{
			if (this.selectedMenuType != value)
			{
				this.selectedMenuType = value;
				this.signalBus.Dispatch(EquipmentSelectorAPI.UPDATED);
			}
		}
	}

	public EquipmentTypes SelectedEquipType
	{
		get
		{
			return this.selectedEquipType;
		}
		set
		{
			if (this.selectedEquipType != value)
			{
				this.selectedEquipType = value;
				this.SelectedEquipment = null;
				this.userInventory.MarkAsNotNewGlobal(value, true);
				this.signalBus.Dispatch(EquipmentSelectorAPI.UPDATED);
			}
		}
	}

	public EquippableItem SelectedEquipment
	{
		get
		{
			return this.selectedEquipment;
		}
		set
		{
			if (this.selectedEquipment != value)
			{
				this.selectedEquipment = value;
				if (this.selectedEquipment != null)
				{
					this.selectedMenuType = EquipmentSelectorModule.MenuType.ITEM;
				}
				this.markNotNewOnSelectEquipment();
				this.signalBus.Dispatch(EquipmentSelectorAPI.UPDATED);
			}
		}
	}

	public CharacterID SelectedCharacter
	{
		get
		{
			return this.characterEquipViewAPI.SelectedCharacter;
		}
	}

	[PostConstruct]
	public void Init()
	{
		this.configureEquipment();
	}

	private void configureEquipment()
	{
		this.equipmentTypes = new List<EquipTypeDefinition>
		{
			new EquipTypeDefinition(EquipmentTypes.SKIN),
			new EquipTypeDefinition(EquipmentTypes.EMOTE),
			new EquipTypeDefinition(EquipmentTypes.VOICE_TAUNT),
			new EquipTypeDefinition(EquipmentTypes.HOLOGRAM),
			new EquipTypeDefinition(EquipmentTypes.VICTORY_POSE),
			new EquipTypeDefinition(EquipmentTypes.PLATFORM),
			new EquipTypeDefinition(EquipmentTypes.NETSUKE),
			new EquipTypeDefinition(EquipmentTypes.TOKEN),
			new EquipTypeDefinition(EquipmentTypes.ANNOUNCERS),
			new EquipTypeDefinition(EquipmentTypes.LOADING_SCREEN),
			new EquipTypeDefinition(EquipmentTypes.BLAST_ZONE),
			new EquipTypeDefinition(EquipmentTypes.PLAYER_ICON)
		}.ToArray();
		this.equipTypeDefIndex = new Dictionary<EquipmentTypes, EquipTypeDefinition>();
		EquipTypeDefinition[] array = this.equipmentTypes;
		for (int i = 0; i < array.Length; i++)
		{
			EquipTypeDefinition equipTypeDefinition = array[i];
			this.equipTypeDefIndex[equipTypeDefinition.type] = equipTypeDefinition;
		}
	}

	public void LoadTypeList(EquipmentTypes[] validTypes)
	{
		this.validTypes = validTypes;
		List<EquipTypeDefinition> list = new List<EquipTypeDefinition>();
		for (int i = 0; i < validTypes.Length; i++)
		{
			EquipmentTypes key = validTypes[i];
			list.Add(this.equipTypeDefIndex[key]);
		}
		this.validEquipDefs = list.ToArray();
	}

	public void MapEquipTypeIcon(EquipmentTypes type, Sprite icon)
	{
		this.equipTypeDefIndex[type].icon = icon;
	}

	public EquipTypeDefinition[] GetValidEquipTypes()
	{
		return this.validEquipDefs;
	}

	private EquipmentTypes getDefaultEquipType()
	{
		return this.validTypes[0];
	}

	private void markNotNewOnSelectEquipment()
	{
		if (this.selectedEquipment != null && this.HasItem(this.selectedEquipment.id))
		{
			this.MarkAsNotNew(this.selectedEquipment.id);
		}
	}

	public bool HasItem(EquipmentID id)
	{
		return this.userInventory.HasItem(id);
	}

	public void MarkAsNotNew(EquipmentID id)
	{
		this.userInventory.MarkAsNotNew(id, true);
	}

	public bool CanEquip(EquippableItem item)
	{
		if (!this.userInventory.HasItem(item.id))
		{
			return false;
		}
		switch (this.equipMethodMap.GetMethod(item.type))
		{
		case EquipMethod.CHARACTER:
			return this.userCharacterUnlockModel.IsUnlocked(this.SelectedCharacter) && !this.userCharacterEquippedModel.IsEquipped(item, this.SelectedCharacter, this.storeAPI.Port);
		case EquipMethod.TAUNT:
			return this.userCharacterUnlockModel.IsUnlocked(this.SelectedCharacter);
		case EquipMethod.NETSUKE:
			return true;
		}
		return !this.userGlobalEquippedModel.IsEquipped(item, this.storeAPI.Port);
	}

	public bool HasPrice(EquippableItem item)
	{
		return item.price > 0;
	}

	public bool IsEquipped(EquippableItem item)
	{
		switch (this.equipMethodMap.GetMethod(item.type))
		{
		case EquipMethod.CHARACTER:
			return this.userCharacterEquippedModel.IsEquipped(item, this.SelectedCharacter, this.storeAPI.Port);
		case EquipMethod.TAUNT:
			return this.userTauntsModel.IsEquipped(item, this.SelectedCharacter, this.storeAPI.Port);
		}
		return this.userGlobalEquippedModel.IsEquipped(item, this.storeAPI.Port);
	}

	public bool IsNew(EquipmentID id)
	{
		return this.userInventory.IsNew(id);
	}

	public string GetLocalizedItemName(EquippableItem item)
	{
		return this.equipmentModel.GetLocalizedItemName(item);
	}
}

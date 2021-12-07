// Decompile from assembly: Assembly-CSharp.dll

using System;

public class EquipFlow : IEquipFlow
{
	public static string NETSUKE = "EquipFlow.NETSUKE";

	private EquippableItem item;

	private CharacterID characterID;

	[Inject]
	public IEquipMethodMap equipMethodMap
	{
		get;
		set;
	}

	[Inject]
	public IDialogController dialogController
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
	public IUserGlobalEquippedModel userGlobalEquippedModel
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
	public IInputBlocker inputBlocker
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

	[Inject]
	public AudioManager audioManager
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

	[Inject]
	public IStoreAPI storeAPI
	{
		get;
		set;
	}

	public bool IsValid(EquippableItem item, CharacterID characterID)
	{
		switch (this.equipMethodMap.GetMethod(item.type))
		{
		case EquipMethod.CHARACTER:
		case EquipMethod.TAUNT:
			return characterID != CharacterID.Any || item.type == EquipmentTypes.PLATFORM;
		case EquipMethod.NONE:
			return false;
		}
		return true;
	}

	public void Start(EquippableItem item, CharacterID characterID)
	{
		this.item = item;
		this.characterID = characterID;
		this.userInventory.MarkAsNotNew(item.id, true);
		switch (this.equipMethodMap.GetMethod(item.type))
		{
		case EquipMethod.CHARACTER:
			this.equipCharacter();
			return;
		case EquipMethod.TAUNT:
			this.equipTauntDialog();
			return;
		case EquipMethod.NETSUKE:
			this.signalBus.Dispatch(EquipFlow.NETSUKE);
			return;
		}
		this.equipBasic();
	}

	public void StartFromUnboxing(EquippableItem item, CharacterID characterID)
	{
		if (this.equipMethodMap.GetMethod(item.type) == EquipMethod.NETSUKE)
		{
			this.quickEquipNetsuke(item);
		}
		else
		{
			this.Start(item, characterID);
		}
	}

	private void quickEquipNetsuke(EquippableItem item)
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userInventory.MarkAsNotNew(item.id, true);
		int openNetsukeSlot = this.userGlobalEquippedModel.GetOpenNetsukeSlot(this.storeAPI.Port);
		this.userGlobalEquippedModel.EquipNetsuke(item, openNetsukeSlot, this.storeAPI.Port, true);
	}

	private void equipCharacter()
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userCharacterEquippedModel.Equip(this.item, this.characterID, this.storeAPI.Port, true);
	}

	private void equipBasic()
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userGlobalEquippedModel.Equip(this.item, this.storeAPI.Port, true);
	}

	private void equipTauntDialog()
	{
		EquipTauntDialog equipTauntDialog = this.dialogController.ShowEquipDialog();
		equipTauntDialog.Setup(this.item, this.characterID);
	}
}

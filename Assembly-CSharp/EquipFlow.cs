using System;

// Token: 0x020009F8 RID: 2552
public class EquipFlow : IEquipFlow
{
	// Token: 0x17001179 RID: 4473
	// (get) Token: 0x06004917 RID: 18711 RVA: 0x0013B4F9 File Offset: 0x001398F9
	// (set) Token: 0x06004918 RID: 18712 RVA: 0x0013B501 File Offset: 0x00139901
	[Inject]
	public IEquipMethodMap equipMethodMap { get; set; }

	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06004919 RID: 18713 RVA: 0x0013B50A File Offset: 0x0013990A
	// (set) Token: 0x0600491A RID: 18714 RVA: 0x0013B512 File Offset: 0x00139912
	[Inject]
	public IDialogController dialogController { get; set; }

	// Token: 0x1700117B RID: 4475
	// (get) Token: 0x0600491B RID: 18715 RVA: 0x0013B51B File Offset: 0x0013991B
	// (set) Token: 0x0600491C RID: 18716 RVA: 0x0013B523 File Offset: 0x00139923
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x1700117C RID: 4476
	// (get) Token: 0x0600491D RID: 18717 RVA: 0x0013B52C File Offset: 0x0013992C
	// (set) Token: 0x0600491E RID: 18718 RVA: 0x0013B534 File Offset: 0x00139934
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel { get; set; }

	// Token: 0x1700117D RID: 4477
	// (get) Token: 0x0600491F RID: 18719 RVA: 0x0013B53D File Offset: 0x0013993D
	// (set) Token: 0x06004920 RID: 18720 RVA: 0x0013B545 File Offset: 0x00139945
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x1700117E RID: 4478
	// (get) Token: 0x06004921 RID: 18721 RVA: 0x0013B54E File Offset: 0x0013994E
	// (set) Token: 0x06004922 RID: 18722 RVA: 0x0013B556 File Offset: 0x00139956
	[Inject]
	public IInputBlocker inputBlocker { get; set; }

	// Token: 0x1700117F RID: 4479
	// (get) Token: 0x06004923 RID: 18723 RVA: 0x0013B55F File Offset: 0x0013995F
	// (set) Token: 0x06004924 RID: 18724 RVA: 0x0013B567 File Offset: 0x00139967
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001180 RID: 4480
	// (get) Token: 0x06004925 RID: 18725 RVA: 0x0013B570 File Offset: 0x00139970
	// (set) Token: 0x06004926 RID: 18726 RVA: 0x0013B578 File Offset: 0x00139978
	[Inject]
	public AudioManager audioManager { get; set; }

	// Token: 0x17001181 RID: 4481
	// (get) Token: 0x06004927 RID: 18727 RVA: 0x0013B581 File Offset: 0x00139981
	// (set) Token: 0x06004928 RID: 18728 RVA: 0x0013B589 File Offset: 0x00139989
	[Inject]
	public ConfigData config { get; set; }

	// Token: 0x17001182 RID: 4482
	// (get) Token: 0x06004929 RID: 18729 RVA: 0x0013B592 File Offset: 0x00139992
	// (set) Token: 0x0600492A RID: 18730 RVA: 0x0013B59A File Offset: 0x0013999A
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x0600492B RID: 18731 RVA: 0x0013B5A4 File Offset: 0x001399A4
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

	// Token: 0x0600492C RID: 18732 RVA: 0x0013B5FC File Offset: 0x001399FC
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

	// Token: 0x0600492D RID: 18733 RVA: 0x0013B68C File Offset: 0x00139A8C
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

	// Token: 0x0600492E RID: 18734 RVA: 0x0013B6BC File Offset: 0x00139ABC
	private void quickEquipNetsuke(EquippableItem item)
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userInventory.MarkAsNotNew(item.id, true);
		int openNetsukeSlot = this.userGlobalEquippedModel.GetOpenNetsukeSlot(this.storeAPI.Port);
		this.userGlobalEquippedModel.EquipNetsuke(item, openNetsukeSlot, this.storeAPI.Port, true);
	}

	// Token: 0x0600492F RID: 18735 RVA: 0x0013B71D File Offset: 0x00139B1D
	private void equipCharacter()
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userCharacterEquippedModel.Equip(this.item, this.characterID, this.storeAPI.Port, true);
	}

	// Token: 0x06004930 RID: 18736 RVA: 0x0013B754 File Offset: 0x00139B54
	private void equipBasic()
	{
		this.audioManager.PlayMenuSound(SoundKey.store_itemEquip, 0f);
		this.userGlobalEquippedModel.Equip(this.item, this.storeAPI.Port, true);
	}

	// Token: 0x06004931 RID: 18737 RVA: 0x0013B788 File Offset: 0x00139B88
	private void equipTauntDialog()
	{
		EquipTauntDialog equipTauntDialog = this.dialogController.ShowEquipDialog();
		equipTauntDialog.Setup(this.item, this.characterID);
	}

	// Token: 0x04003043 RID: 12355
	public static string NETSUKE = "EquipFlow.NETSUKE";

	// Token: 0x0400304E RID: 12366
	private EquippableItem item;

	// Token: 0x0400304F RID: 12367
	private CharacterID characterID;
}

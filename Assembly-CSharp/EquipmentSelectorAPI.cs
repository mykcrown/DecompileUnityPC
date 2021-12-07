using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020009FA RID: 2554
public class EquipmentSelectorAPI : IEquipModuleAPI
{
	// Token: 0x17001183 RID: 4483
	// (get) Token: 0x06004937 RID: 18743 RVA: 0x0013B7C7 File Offset: 0x00139BC7
	// (set) Token: 0x06004938 RID: 18744 RVA: 0x0013B7CF File Offset: 0x00139BCF
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001184 RID: 4484
	// (get) Token: 0x06004939 RID: 18745 RVA: 0x0013B7D8 File Offset: 0x00139BD8
	// (set) Token: 0x0600493A RID: 18746 RVA: 0x0013B7E0 File Offset: 0x00139BE0
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001185 RID: 4485
	// (get) Token: 0x0600493B RID: 18747 RVA: 0x0013B7E9 File Offset: 0x00139BE9
	// (set) Token: 0x0600493C RID: 18748 RVA: 0x0013B7F1 File Offset: 0x00139BF1
	[Inject]
	public IUserInventory userInventory { get; set; }

	// Token: 0x17001186 RID: 4486
	// (get) Token: 0x0600493D RID: 18749 RVA: 0x0013B7FA File Offset: 0x00139BFA
	// (set) Token: 0x0600493E RID: 18750 RVA: 0x0013B802 File Offset: 0x00139C02
	[Inject]
	public IEquipMethodMap equipMethodMap { get; set; }

	// Token: 0x17001187 RID: 4487
	// (get) Token: 0x0600493F RID: 18751 RVA: 0x0013B80B File Offset: 0x00139C0B
	// (set) Token: 0x06004940 RID: 18752 RVA: 0x0013B813 File Offset: 0x00139C13
	[Inject]
	public IUserCharacterEquippedModel userCharacterEquippedModel { get; set; }

	// Token: 0x17001188 RID: 4488
	// (get) Token: 0x06004941 RID: 18753 RVA: 0x0013B81C File Offset: 0x00139C1C
	// (set) Token: 0x06004942 RID: 18754 RVA: 0x0013B824 File Offset: 0x00139C24
	[Inject]
	public IUserCharacterUnlockModel userCharacterUnlockModel { get; set; }

	// Token: 0x17001189 RID: 4489
	// (get) Token: 0x06004943 RID: 18755 RVA: 0x0013B82D File Offset: 0x00139C2D
	// (set) Token: 0x06004944 RID: 18756 RVA: 0x0013B835 File Offset: 0x00139C35
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel { get; set; }

	// Token: 0x1700118A RID: 4490
	// (get) Token: 0x06004945 RID: 18757 RVA: 0x0013B83E File Offset: 0x00139C3E
	// (set) Token: 0x06004946 RID: 18758 RVA: 0x0013B846 File Offset: 0x00139C46
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x1700118B RID: 4491
	// (get) Token: 0x06004947 RID: 18759 RVA: 0x0013B84F File Offset: 0x00139C4F
	// (set) Token: 0x06004948 RID: 18760 RVA: 0x0013B857 File Offset: 0x00139C57
	[Inject]
	public IUserTauntsModel userTauntsModel { get; set; }

	// Token: 0x1700118C RID: 4492
	// (get) Token: 0x06004949 RID: 18761 RVA: 0x0013B860 File Offset: 0x00139C60
	// (set) Token: 0x0600494A RID: 18762 RVA: 0x0013B868 File Offset: 0x00139C68
	[Inject]
	public ICharacterEquipViewAPI characterEquipViewAPI { get; set; }

	// Token: 0x0600494B RID: 18763 RVA: 0x0013B871 File Offset: 0x00139C71
	[PostConstruct]
	public void Init()
	{
		this.configureEquipment();
	}

	// Token: 0x0600494C RID: 18764 RVA: 0x0013B87C File Offset: 0x00139C7C
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
		foreach (EquipTypeDefinition equipTypeDefinition in this.equipmentTypes)
		{
			this.equipTypeDefIndex[equipTypeDefinition.type] = equipTypeDefinition;
		}
	}

	// Token: 0x0600494D RID: 18765 RVA: 0x0013B96C File Offset: 0x00139D6C
	public void LoadTypeList(EquipmentTypes[] validTypes)
	{
		this.validTypes = validTypes;
		List<EquipTypeDefinition> list = new List<EquipTypeDefinition>();
		foreach (EquipmentTypes key in validTypes)
		{
			list.Add(this.equipTypeDefIndex[key]);
		}
		this.validEquipDefs = list.ToArray();
	}

	// Token: 0x0600494E RID: 18766 RVA: 0x0013B9BE File Offset: 0x00139DBE
	public void MapEquipTypeIcon(EquipmentTypes type, Sprite icon)
	{
		this.equipTypeDefIndex[type].icon = icon;
	}

	// Token: 0x0600494F RID: 18767 RVA: 0x0013B9D2 File Offset: 0x00139DD2
	public EquipTypeDefinition[] GetValidEquipTypes()
	{
		return this.validEquipDefs;
	}

	// Token: 0x06004950 RID: 18768 RVA: 0x0013B9DA File Offset: 0x00139DDA
	private EquipmentTypes getDefaultEquipType()
	{
		return this.validTypes[0];
	}

	// Token: 0x1700118D RID: 4493
	// (get) Token: 0x06004951 RID: 18769 RVA: 0x0013B9E4 File Offset: 0x00139DE4
	// (set) Token: 0x06004952 RID: 18770 RVA: 0x0013B9EC File Offset: 0x00139DEC
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

	// Token: 0x1700118E RID: 4494
	// (get) Token: 0x06004953 RID: 18771 RVA: 0x0013BA11 File Offset: 0x00139E11
	// (set) Token: 0x06004954 RID: 18772 RVA: 0x0013BA19 File Offset: 0x00139E19
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

	// Token: 0x1700118F RID: 4495
	// (get) Token: 0x06004955 RID: 18773 RVA: 0x0013BA52 File Offset: 0x00139E52
	// (set) Token: 0x06004956 RID: 18774 RVA: 0x0013BA5A File Offset: 0x00139E5A
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

	// Token: 0x17001190 RID: 4496
	// (get) Token: 0x06004957 RID: 18775 RVA: 0x0013BA97 File Offset: 0x00139E97
	public CharacterID SelectedCharacter
	{
		get
		{
			return this.characterEquipViewAPI.SelectedCharacter;
		}
	}

	// Token: 0x06004958 RID: 18776 RVA: 0x0013BAA4 File Offset: 0x00139EA4
	private void markNotNewOnSelectEquipment()
	{
		if (this.selectedEquipment != null && this.HasItem(this.selectedEquipment.id))
		{
			this.MarkAsNotNew(this.selectedEquipment.id);
		}
	}

	// Token: 0x06004959 RID: 18777 RVA: 0x0013BAD8 File Offset: 0x00139ED8
	public bool HasItem(EquipmentID id)
	{
		return this.userInventory.HasItem(id);
	}

	// Token: 0x0600495A RID: 18778 RVA: 0x0013BAE6 File Offset: 0x00139EE6
	public void MarkAsNotNew(EquipmentID id)
	{
		this.userInventory.MarkAsNotNew(id, true);
	}

	// Token: 0x0600495B RID: 18779 RVA: 0x0013BAF8 File Offset: 0x00139EF8
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

	// Token: 0x0600495C RID: 18780 RVA: 0x0013BBB2 File Offset: 0x00139FB2
	public bool HasPrice(EquippableItem item)
	{
		return item.price > 0;
	}

	// Token: 0x0600495D RID: 18781 RVA: 0x0013BBC0 File Offset: 0x00139FC0
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

	// Token: 0x0600495E RID: 18782 RVA: 0x0013BC4D File Offset: 0x0013A04D
	public bool IsNew(EquipmentID id)
	{
		return this.userInventory.IsNew(id);
	}

	// Token: 0x0600495F RID: 18783 RVA: 0x0013BC5B File Offset: 0x0013A05B
	public string GetLocalizedItemName(EquippableItem item)
	{
		return this.equipmentModel.GetLocalizedItemName(item);
	}

	// Token: 0x04003050 RID: 12368
	public static string UPDATED = "EquipmentSelectorAPI.UPDATED";

	// Token: 0x0400305B RID: 12379
	private Dictionary<EquipmentTypes, EquipTypeDefinition> equipTypeDefIndex;

	// Token: 0x0400305C RID: 12380
	private EquipTypeDefinition[] equipmentTypes;

	// Token: 0x0400305D RID: 12381
	private EquipmentTypes[] validTypes;

	// Token: 0x0400305E RID: 12382
	private EquipTypeDefinition[] validEquipDefs;

	// Token: 0x0400305F RID: 12383
	private EquipmentTypes selectedEquipType;

	// Token: 0x04003060 RID: 12384
	private EquippableItem selectedEquipment;

	// Token: 0x04003061 RID: 12385
	private EquipmentSelectorModule.MenuType selectedMenuType;
}

using System;
using System.Collections.Generic;

// Token: 0x020009F6 RID: 2550
public class NetsukeEquipViewAPI : INetsukeEquipViewAPI
{
	// Token: 0x1700116E RID: 4462
	// (get) Token: 0x060048F6 RID: 18678 RVA: 0x0013B275 File Offset: 0x00139675
	// (set) Token: 0x060048F7 RID: 18679 RVA: 0x0013B27D File Offset: 0x0013967D
	[Inject]
	public IUserGlobalEquippedModel userGlobalEquippedModel { get; set; }

	// Token: 0x1700116F RID: 4463
	// (get) Token: 0x060048F8 RID: 18680 RVA: 0x0013B286 File Offset: 0x00139686
	// (set) Token: 0x060048F9 RID: 18681 RVA: 0x0013B28E File Offset: 0x0013968E
	[Inject]
	public IEquipmentModel equipmentModel { get; set; }

	// Token: 0x17001170 RID: 4464
	// (get) Token: 0x060048FA RID: 18682 RVA: 0x0013B297 File Offset: 0x00139697
	// (set) Token: 0x060048FB RID: 18683 RVA: 0x0013B29F File Offset: 0x0013969F
	[Inject]
	public ISignalBus signalBus { get; set; }

	// Token: 0x17001171 RID: 4465
	// (get) Token: 0x060048FC RID: 18684 RVA: 0x0013B2A8 File Offset: 0x001396A8
	// (set) Token: 0x060048FD RID: 18685 RVA: 0x0013B2B0 File Offset: 0x001396B0
	[Inject]
	public IStoreAPI storeAPI { get; set; }

	// Token: 0x17001172 RID: 4466
	// (get) Token: 0x060048FE RID: 18686 RVA: 0x0013B2B9 File Offset: 0x001396B9
	// (set) Token: 0x060048FF RID: 18687 RVA: 0x0013B2C1 File Offset: 0x001396C1
	[Inject("CollectiblesEquipView")]
	public IEquipModuleAPI equipModuleAPI { get; set; }

	// Token: 0x06004900 RID: 18688 RVA: 0x0013B2CC File Offset: 0x001396CC
	public void BeginEdit()
	{
		this.changesTable.Clear();
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			EquipmentID equippedNetsuke = this.userGlobalEquippedModel.GetEquippedNetsuke(i, this.storeAPI.Port);
			if (equippedNetsuke.IsNull())
			{
				this.changesTable[i] = null;
			}
			else
			{
				this.changesTable[i] = this.equipmentModel.GetItem(equippedNetsuke);
			}
		}
	}

	// Token: 0x06004901 RID: 18689 RVA: 0x0013B348 File Offset: 0x00139748
	public void SaveEdit()
	{
		for (int i = 0; i < UserGlobalEquippedModel.NETSUKE_SLOTS; i++)
		{
			this.userGlobalEquippedModel.EquipNetsuke(this.changesTable[i], i, this.storeAPI.Port, true);
		}
	}

	// Token: 0x06004902 RID: 18690 RVA: 0x0013B38F File Offset: 0x0013978F
	public void DiscardEdit()
	{
		this.changesTable.Clear();
		this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
	}

	// Token: 0x06004903 RID: 18691 RVA: 0x0013B3AC File Offset: 0x001397AC
	public void EquipNetsuke(EquippableItem item, int index)
	{
		if (this.changesTable[index] == item)
		{
			this.changesTable[index] = null;
		}
		else
		{
			this.changesTable[index] = item;
		}
		this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
	}

	// Token: 0x06004904 RID: 18692 RVA: 0x0013B3FC File Offset: 0x001397FC
	public EquippableItem GetEquippedNetsuke(int index)
	{
		EquippableItem result;
		this.changesTable.TryGetValue(index, out result);
		return result;
	}

	// Token: 0x17001173 RID: 4467
	// (get) Token: 0x06004905 RID: 18693 RVA: 0x0013B419 File Offset: 0x00139819
	public int SelectedIndex
	{
		get
		{
			return this.selectedIndex;
		}
	}

	// Token: 0x17001174 RID: 4468
	// (get) Token: 0x06004906 RID: 18694 RVA: 0x0013B421 File Offset: 0x00139821
	// (set) Token: 0x06004907 RID: 18695 RVA: 0x0013B42C File Offset: 0x0013982C
	public int SpinIndex
	{
		get
		{
			return this.spinIndex;
		}
		set
		{
			if (this.spinIndex != value)
			{
				int num = value - this.spinIndex;
				this.spinIndex = value;
				this.selectedIndex += num;
				if (this.selectedIndex < 0)
				{
					this.selectedIndex += UserGlobalEquippedModel.NETSUKE_SLOTS;
				}
				else if (this.selectedIndex >= UserGlobalEquippedModel.NETSUKE_SLOTS)
				{
					this.selectedIndex -= UserGlobalEquippedModel.NETSUKE_SLOTS;
				}
				this.signalBus.Dispatch(NetsukeEquipViewAPI.UPDATED);
			}
		}
	}

	// Token: 0x17001175 RID: 4469
	// (get) Token: 0x06004908 RID: 18696 RVA: 0x0013B4B8 File Offset: 0x001398B8
	public EquippableItem SelectedItem
	{
		get
		{
			return this.equipModuleAPI.SelectedEquipment;
		}
	}

	// Token: 0x06004909 RID: 18697 RVA: 0x0013B4C5 File Offset: 0x001398C5
	public void TurnLeft()
	{
		this.SpinIndex--;
	}

	// Token: 0x0600490A RID: 18698 RVA: 0x0013B4D5 File Offset: 0x001398D5
	public void TurnRight()
	{
		this.SpinIndex++;
	}

	// Token: 0x0400303A RID: 12346
	public static string UPDATED = "NetsukeEquipViewAPI.UPDATED";

	// Token: 0x04003040 RID: 12352
	private int spinIndex;

	// Token: 0x04003041 RID: 12353
	private int selectedIndex;

	// Token: 0x04003042 RID: 12354
	private Dictionary<int, EquippableItem> changesTable = new Dictionary<int, EquippableItem>();
}

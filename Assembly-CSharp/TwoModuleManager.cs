using System;
using System.Collections.Generic;

// Token: 0x02000A02 RID: 2562
public class TwoModuleManager : IEquipmentSelectorModule
{
	// Token: 0x060049FC RID: 18940 RVA: 0x0013DEF5 File Offset: 0x0013C2F5
	public void AddModule(IEquipmentSelectorModule module)
	{
		this.allModules.Add(module);
	}

	// Token: 0x060049FD RID: 18941 RVA: 0x0013DF03 File Offset: 0x0013C303
	public void SetActive(EquipmentSelectorModule module)
	{
		this.currentlyActive = module;
		this.sendEventsTo.Clear();
		this.sendEventsTo.Add(module);
	}

	// Token: 0x060049FE RID: 18942 RVA: 0x0013DF23 File Offset: 0x0013C323
	public EquipmentSelectorModule GetCurrent()
	{
		return this.currentlyActive;
	}

	// Token: 0x060049FF RID: 18943 RVA: 0x0013DF2C File Offset: 0x0013C32C
	public void LoadItems(List<EquippableItem> items)
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.allModules)
		{
			equipmentSelectorModule.LoadItems(items);
		}
	}

	// Token: 0x06004A00 RID: 18944 RVA: 0x0013DF88 File Offset: 0x0013C388
	public void Activate()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.allModules)
		{
			equipmentSelectorModule.Activate();
		}
	}

	// Token: 0x06004A01 RID: 18945 RVA: 0x0013DFE4 File Offset: 0x0013C3E4
	public void OnDrawComplete()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.allModules)
		{
			equipmentSelectorModule.OnDrawComplete();
		}
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x0013E040 File Offset: 0x0013C440
	public bool OnCancelPressed()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			flag |= equipmentSelectorModule.OnCancelPressed();
		}
		return flag;
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x0013E0A4 File Offset: 0x0013C4A4
	public bool OnLeft()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			flag |= equipmentSelectorModule.OnLeft();
		}
		return flag;
	}

	// Token: 0x06004A04 RID: 18948 RVA: 0x0013E108 File Offset: 0x0013C508
	public bool OnRight()
	{
		bool flag = false;
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			flag |= equipmentSelectorModule.OnRight();
		}
		return flag;
	}

	// Token: 0x06004A05 RID: 18949 RVA: 0x0013E16C File Offset: 0x0013C56C
	public void ForceRedraws()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.ForceRedraws();
		}
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x0013E1C8 File Offset: 0x0013C5C8
	public void BeginMenuFocus()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.BeginMenuFocus();
		}
	}

	// Token: 0x06004A07 RID: 18951 RVA: 0x0013E224 File Offset: 0x0013C624
	public void OnMouseModeUpdate()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.OnMouseModeUpdate();
		}
	}

	// Token: 0x06004A08 RID: 18952 RVA: 0x0013E280 File Offset: 0x0013C680
	public void SyncButtonModeSelection()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.SyncButtonModeSelection();
		}
	}

	// Token: 0x06004A09 RID: 18953 RVA: 0x0013E2DC File Offset: 0x0013C6DC
	public void RebuildList()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.RebuildList();
		}
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x0013E338 File Offset: 0x0013C738
	public void ReleaseSelections()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.ReleaseSelections();
		}
	}

	// Token: 0x06004A0B RID: 18955 RVA: 0x0013E394 File Offset: 0x0013C794
	public void DeselectEquipment()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.DeselectEquipment();
		}
	}

	// Token: 0x06004A0C RID: 18956 RVA: 0x0013E3F0 File Offset: 0x0013C7F0
	public void EnterFromRight()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.EnterFromRight();
		}
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x0013E44C File Offset: 0x0013C84C
	public void EnterFromBottom()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.EnterFromBottom();
		}
	}

	// Token: 0x06004A0E RID: 18958 RVA: 0x0013E4A8 File Offset: 0x0013C8A8
	public void OnYButton()
	{
		foreach (IEquipmentSelectorModule equipmentSelectorModule in this.sendEventsTo)
		{
			equipmentSelectorModule.OnYButton();
		}
	}

	// Token: 0x040030B3 RID: 12467
	private List<IEquipmentSelectorModule> allModules = new List<IEquipmentSelectorModule>();

	// Token: 0x040030B4 RID: 12468
	private EquipmentSelectorModule currentlyActive;

	// Token: 0x040030B5 RID: 12469
	private List<IEquipmentSelectorModule> sendEventsTo = new List<IEquipmentSelectorModule>();
}

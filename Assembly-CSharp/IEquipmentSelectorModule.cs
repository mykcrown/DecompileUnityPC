using System;
using System.Collections.Generic;

// Token: 0x02000A00 RID: 2560
public interface IEquipmentSelectorModule
{
	// Token: 0x060049DC RID: 18908
	void LoadItems(List<EquippableItem> items);

	// Token: 0x060049DD RID: 18909
	void Activate();

	// Token: 0x060049DE RID: 18910
	void ForceRedraws();

	// Token: 0x060049DF RID: 18911
	void BeginMenuFocus();

	// Token: 0x060049E0 RID: 18912
	void OnMouseModeUpdate();

	// Token: 0x060049E1 RID: 18913
	void SyncButtonModeSelection();

	// Token: 0x060049E2 RID: 18914
	void OnDrawComplete();

	// Token: 0x060049E3 RID: 18915
	void RebuildList();

	// Token: 0x060049E4 RID: 18916
	void DeselectEquipment();

	// Token: 0x060049E5 RID: 18917
	void ReleaseSelections();

	// Token: 0x060049E6 RID: 18918
	void EnterFromRight();

	// Token: 0x060049E7 RID: 18919
	void EnterFromBottom();

	// Token: 0x060049E8 RID: 18920
	bool OnCancelPressed();

	// Token: 0x060049E9 RID: 18921
	bool OnLeft();

	// Token: 0x060049EA RID: 18922
	bool OnRight();

	// Token: 0x060049EB RID: 18923
	void OnYButton();
}

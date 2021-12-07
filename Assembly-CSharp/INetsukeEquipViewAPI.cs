using System;

// Token: 0x020009F7 RID: 2551
public interface INetsukeEquipViewAPI
{
	// Token: 0x0600490C RID: 18700
	void EquipNetsuke(EquippableItem item, int index);

	// Token: 0x0600490D RID: 18701
	EquippableItem GetEquippedNetsuke(int index);

	// Token: 0x0600490E RID: 18702
	void BeginEdit();

	// Token: 0x0600490F RID: 18703
	void SaveEdit();

	// Token: 0x06004910 RID: 18704
	void DiscardEdit();

	// Token: 0x06004911 RID: 18705
	void TurnLeft();

	// Token: 0x06004912 RID: 18706
	void TurnRight();

	// Token: 0x17001176 RID: 4470
	// (get) Token: 0x06004913 RID: 18707
	int SelectedIndex { get; }

	// Token: 0x17001177 RID: 4471
	// (get) Token: 0x06004914 RID: 18708
	int SpinIndex { get; }

	// Token: 0x17001178 RID: 4472
	// (get) Token: 0x06004915 RID: 18709
	EquippableItem SelectedItem { get; }
}

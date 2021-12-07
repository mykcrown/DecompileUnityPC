using System;

// Token: 0x02000A05 RID: 2565
public interface IUnlockEquipmentFlow
{
	// Token: 0x06004A39 RID: 19001
	void Start(EquippableItem item, Action closeCallback, Action equipCallback);

	// Token: 0x06004A3A RID: 19002
	void StartWithTimer(EquippableItem item, Action closeCallback, Action equipCallback, float endTime);
}

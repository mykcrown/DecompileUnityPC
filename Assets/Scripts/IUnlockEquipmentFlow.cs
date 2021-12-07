// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUnlockEquipmentFlow
{
	void Start(EquippableItem item, Action closeCallback, Action equipCallback);

	void StartWithTimer(EquippableItem item, Action closeCallback, Action equipCallback, float endTime);
}

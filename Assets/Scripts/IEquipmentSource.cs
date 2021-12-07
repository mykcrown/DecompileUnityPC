// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IEquipmentSource
{
	bool IsReady();

	EquippableItem[] GetAll();
}

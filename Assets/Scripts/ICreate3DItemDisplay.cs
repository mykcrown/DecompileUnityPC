// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ICreate3DItemDisplay
{
	Item3DPreview CreateDisplay(EquippableItem item);

	Item3DPreview CreateDefault(EquipmentTypes type);

	void DestroyPreview(Item3DPreview preview);
}

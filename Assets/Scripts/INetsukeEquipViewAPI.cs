// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface INetsukeEquipViewAPI
{
	int SelectedIndex
	{
		get;
	}

	int SpinIndex
	{
		get;
	}

	EquippableItem SelectedItem
	{
		get;
	}

	void EquipNetsuke(EquippableItem item, int index);

	EquippableItem GetEquippedNetsuke(int index);

	void BeginEdit();

	void SaveEdit();

	void DiscardEdit();

	void TurnLeft();

	void TurnRight();
}

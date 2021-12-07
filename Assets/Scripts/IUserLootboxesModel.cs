// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface IUserLootboxesModel
{
	void Add(int itemId, int quantity);

	int GetQuantity(int itemId);

	int GetTotalQuantity();

	int GetNextBoxId();
}

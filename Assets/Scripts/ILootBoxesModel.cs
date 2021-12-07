// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILootBoxesModel
{
	LootBoxPackage GetBoxToBuy(ulong packageId);

	LootBoxPackage[] GetPackages();
}

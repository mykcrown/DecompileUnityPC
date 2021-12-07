// Decompile from assembly: Assembly-CSharp.dll

using System;

public interface ILootBoxesSource
{
	LootBoxPackage[] GetAllLootBoxes();

	LootBoxPackage GetLootBoxByPackageId(ulong packageId);
}

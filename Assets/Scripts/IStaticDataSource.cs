// Decompile from assembly: Assembly-CSharp.dll

using IconsTool;
using System;

public interface IStaticDataSource
{
	ulong CharacterUnlockTokenId
	{
		get;
	}

	StaticDataReadOnly.StaticItemBase GetLocalItemData(int id);

	StaticDataReadOnly.StaticPackageData GetLocalPackageData(int packageId);

	ulong GetSpectraInPackage(int packageId);

	bool ValidateStaticData();

	ulong GetChecksum();
}

using System;
using IconsTool;

// Token: 0x02000736 RID: 1846
public interface IStaticDataSource
{
	// Token: 0x06002D98 RID: 11672
	StaticDataReadOnly.StaticItemBase GetLocalItemData(int id);

	// Token: 0x06002D99 RID: 11673
	StaticDataReadOnly.StaticPackageData GetLocalPackageData(int packageId);

	// Token: 0x06002D9A RID: 11674
	ulong GetSpectraInPackage(int packageId);

	// Token: 0x17000B24 RID: 2852
	// (get) Token: 0x06002D9B RID: 11675
	ulong CharacterUnlockTokenId { get; }

	// Token: 0x06002D9C RID: 11676
	bool ValidateStaticData();

	// Token: 0x06002D9D RID: 11677
	ulong GetChecksum();
}

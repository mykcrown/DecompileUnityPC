using System;

// Token: 0x02000348 RID: 840
public interface IBakedAnimationDataManager
{
	// Token: 0x060011C5 RID: 4549
	void Set(string characterName, BakedAnimationData data);

	// Token: 0x060011C6 RID: 4550
	BakedAnimationData Get(string characterName);

	// Token: 0x060011C7 RID: 4551
	void OnApplicationQuit();

	// Token: 0x060011C8 RID: 4552
	void Clear();
}
